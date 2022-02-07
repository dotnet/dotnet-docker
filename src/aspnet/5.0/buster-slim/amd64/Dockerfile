ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:buster-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.14 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='5f12f290e7800da808a2bd7deb654f56dfc0d9ed0f6a7f7f065a8f4816f3d334e145f909f016745d9d9c30438c0f69fe4e11c2fce253228a8cdf609c97d26eb6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.14-buster-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.14

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
