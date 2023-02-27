ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.3.23126.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='f08cfa19343c08d9be5c0357d84586176ad98669c5a7e6afed3d0e40f61e3a8d35e8b3756ebed0ded098deadc6e9d47b4f272d90c6e1c3b74c3dc4b494ec35f4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.2-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.3.23126.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
