ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:trixie-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25156.10 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='45587e92712180de2e100afd67f4c799b5940a2289d9b731e1e56c01e3d34d01ea9ae30590a44a12c2556413ffc4c64d231c821aa7df949f042fb43243e9d0c6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-trixie-slim-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25156.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
