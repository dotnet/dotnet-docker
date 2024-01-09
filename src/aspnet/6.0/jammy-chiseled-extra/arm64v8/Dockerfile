ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.26 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='48330ea4d98fc565c9553ea119f56e3e485ca30a0986f43e78335e263d9cc82d17b7ced8115480d1adb33298cbc5cb2b0759bc89d516659c4c59eab9520a2254' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.26-jammy-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.26

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
