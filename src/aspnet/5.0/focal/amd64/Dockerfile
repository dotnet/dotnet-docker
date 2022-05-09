ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.17 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='d8e87804e9e86273c6512785bd5a6f0e834ff3f4bbebc11c4fcdf16ab4fdfabd0d981a756955267c1aa9bbeec596de3728ce9b2e6415d2d80daef0d999a5df6d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.17-focal-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.17

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
