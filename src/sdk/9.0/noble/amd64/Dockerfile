ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Install .NET SDK
RUN curl --fail --show-error --location --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.301/dotnet-sdk-9.0.301-linux-x64.tar.gz \
    && dotnet_sha512='7415a264843d3df78bd57fb2f17074e811e0b193976a45b4d6778d3ebb9266854c4e037c3cdf4b534de7b8c64799b0e58dce9fac6f3f3c6cacb3295555d31d4f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:9.0.6-noble-amd64

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=9.0.301 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Ubuntu-24.04

RUN apt-get update \
    && apt-get install --assume-yes --no-install-recommends \
        curl \
        git \
        libatomic1 \
        wget \
    && rm --recursive --force /var/lib/apt/lists/*

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
RUN powershell_version=7.5.1 \
    && curl --fail --show-error --location --output PowerShell.Linux.x64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='96e6e2b513d5974f62ec46417b62a3a94fce59ac0ef0806ad7dc40f6fd8c15f8064b2e332483e7b180d33e033015e8709eef40a8020144b0d9947f4e5fe2e1fa' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum --check - \
    && mkdir --parents /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln --symbolic /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
