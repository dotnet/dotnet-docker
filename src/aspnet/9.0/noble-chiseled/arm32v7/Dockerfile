ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.1.24416.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='4fd072d2e40e0b7caf1cbf0cdcaba436ee15dd9b3852b872d16eb2f9481231a29183cefa313d23e49a480725c43c0a562b6872f53a15bb5d7ee182d79d0a5ee3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.1-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.1.24416.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
