{{
    _ .NET major version matches the major version of dotnet-monitor ^
    set dotnetMajor to split(PRODUCT_VERSION, ".")[0] ^
    set dotnetMajorMinor to cat(dotnetMajor, ".0") ^
    set monitorMajor to split(PRODUCT_VERSION, ".")[0] ^
    set targetFrameworkMoniker to cat("net", dotnetMajorMinor) ^
    set targetFrameworkMonikerRegex to cat("net", dotnetMajor, "[.]0") ^
    set isMariner to find(OS_VERSION, "cbl-mariner") >= 0 ^
    set isDistroless to find(OS_VERSION, "distroless") >= 0 || find(OS_VERSION, "chiseled") >= 0 ^
    set sdkDistro to when(isDistroless, when(isMariner, cat("cbl-mariner", OS_VERSION_NUMBER), split(OS_VERSION, "-")[0]), OS_VERSION) ^
    set installerBaseTag to cat(VARIABLES[cat("sdk|", dotnetMajorMinor, "|product-version")], "-", sdkDistro, ARCH_TAG_SUFFIX) ^
    set monitorMajorMinor to join(slice(split(PRODUCT_VERSION, "."), 0, 2), ".") ^
    set buildVersion to VARIABLES[cat("monitor|", monitorMajorMinor, "|build-version")] ^
    set monitorVersionVariable to when(find(buildVersion, '-rtm') >= 0 || find(buildVersion, '-servicing') >= 0, "product-version", "build-version") ^
    set monitorVersion to VARIABLES[cat("monitor|", monitorMajorMinor, "|", monitorVersionVariable)] ^
    set imageBaseTag to cat(VARIABLES[cat("dotnet|", dotnetMajorMinor, "|product-version")], "-", OS_VERSION, ARCH_TAG_SUFFIX) ^
    _ When build and product versions are the same and they are stable versions, use account from main branch e.g. dotnetcli
      Otherwise, use the account that is associated with the current branch. ^
    set urlBranch to when(find(monitorVersion, "-") >= 0 || buildVersion != monitorVersion, VARIABLES["branch"], "main") ^
    _ Use the $DOTNET_MONITOR_VERSION variable for the version folder if the build and product versions are the same. ^
    set versionFolder to when(buildVersion != monitorVersion, buildVersion, '$DOTNET_MONITOR_VERSION') ^
    set supportsEndpointDeletion to monitorMajorMinor != "6.1"
}}ARG ASPNET_REPO=mcr.microsoft.com/dotnet/aspnet
ARG SDK_REPO=mcr.microsoft.com/dotnet/sdk

# Installer image
FROM $SDK_REPO:{{installerBaseTag}} AS installer

# Install .NET Monitor
ENV DOTNET_MONITOR_VERSION={{monitorVersion}}
RUN {{InsertTemplate("../Dockerfile.linux.download-file",
        [
            "out-file": "dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg",
            "url": cat(VARIABLES[cat("base-url|", monitorMajorMinor, "-monitor|", urlBranch)], "/diagnostics/monitor/", versionFolder, "/dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg"),
            "sha": VARIABLES[cat("monitor|", monitorMajorMinor, "|sha")],
            "sha-var-name": "dotnetmonitor_sha512"
        ], "    ")}} \
    && dotnet tool install dotnet-monitor --tool-path /app --add-source / --version $DOTNET_MONITOR_VERSION --framework {{targetFrameworkMoniker}} --no-cache \
    # To reduce image size, remove all non-{{targetFrameworkMoniker}} TFMs
    # To do this safely, we need to first find everything that dotnet tool installed named "dotnet-monitor".
    # The /dotnet-monitor/ folder exists under .store which is not a stable constant to rely on. The result is multi stage search:
    # 1. Find any files in /app
    # 2. Match anything that is under a *folder* called 'dotnet-monitor'
    # 3. Match anything from step 2 that isn't in a '/tools/{{targetFrameworkMoniker}}' folder (/tools is the folder we use in a nuget file that is stable)
    # 4. Delete everything from step 3
    && find /app -type f -print | grep -i '.*/dotnet-monitor/.*' | grep -v -i '.*/tools/{{targetFrameworkMonikerRegex}}' | xargs rm -rf \
    # To reduce image size further, remove the non-linux assemblies
    && find /app -type f -print | grep -i '.*/dotnet-monitor/.*' | grep -i -E '/(runtimes|shims)/(win|osx)' | xargs rm -rf \
    # Remove all the empty directories left by the previous step
    && find /app -type d -empty -delete \
    # Allow other users to run the tool
    && chmod 755 /app/dotnet-monitor \
    && rm dotnet-monitor.$DOTNET_MONITOR_VERSION.nupkg


# Monitor image
FROM $ASPNET_REPO:{{imageBaseTag}}

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Disable debugger and profiler diagnostics to avoid diagnosing self.
    COMPlus_EnableDiagnostics=0 \
    # Default Filter
    DefaultProcess__Filters__0__Key=ProcessId \
    DefaultProcess__Filters__0__Value=1 \{{if supportsEndpointDeletion:
    # Remove Unix Domain Socket before starting diagnostic port server
    DiagnosticPort__DeleteEndpointOnStartup=true \}}
    # Logging: JSON format so that analytic platforms can get discrete entry information
    Logging__Console__FormatterName=json \
    # Logging: Use round-trip date/time format without timezone information (always logged in UTC)
    Logging__Console__FormatterOptions__TimestampFormat=yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z' \
    # Logging: Write timestamps using UTC offset (+0:00)
    Logging__Console__FormatterOptions__UseUtcTimestamp=true \
    # Add dotnet-monitor path to front of PATH for easier, prioritized execution
    PATH="/app:${PATH}"

{{InsertTemplate(cat("Dockerfile.entrypoint.monitorV", monitorMajor))}}
