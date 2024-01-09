ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-alpha.1.24058.9 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='0c9c299fee1e07dd973c648872aeef815af7c19403332cc34a0c1ac8b48c50c143e1501b71e5de76b1a21b08d1e3b26ca0727a88165537bb350b95e5bde0d764' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-alpha.1-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-alpha.1.24058.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
