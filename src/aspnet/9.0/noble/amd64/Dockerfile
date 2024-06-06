ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.5.24305.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='a330080e912b6b72ced1b0f9ff4e6ce49cf5e6b38218b07e21de3349a972ec6b14f105051f34ead0d2e950923aa22d268e36931a5c5ceab6eb6cd5495d8095b2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.5-noble-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.5.24305.1

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
