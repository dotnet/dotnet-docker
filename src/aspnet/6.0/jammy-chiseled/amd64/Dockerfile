ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.20 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='891bad6a52a7bcd5afa2a784fe68044d282f6d53fedab4bde6dff8d7d2138a484e947f7a6be156094324b37e9d7e07e87a67622bcf2ea197c2924389edd1d185' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.20-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.20

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
