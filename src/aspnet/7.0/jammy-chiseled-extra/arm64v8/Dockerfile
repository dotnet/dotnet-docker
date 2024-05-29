ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.20 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='dfb1c1bef4d826defd3d995599a5c03e1bf1a64c65d98b675d6c05dbb7380d98233953e68d53fc5ebec60ad4ef75417073fb1fb3256a0176bb964f0e01161f6c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.20-jammy-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.20

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
