ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-preview.3.22117.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='477ca82854801b2c4dc62049478902dc72c0a5e6129d90c2e6472a1e9c25bd1e2794e8158bb6344f11fe694f1195d0cd31140f7b425640ae6d573c976a051cb3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.2-focal-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.3.22117.6

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
