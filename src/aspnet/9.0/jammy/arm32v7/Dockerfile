ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.4.24173.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='75fdb42faa6578aa62fb8b6f1b7602ea28abe1ea8f8bc2b10088de71f11b9a758901b537cde34f61bbd613c4c8ee9d7ec7c47000c9c3869d4f3756c48bd8b3d6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.4-jammy-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24173.3

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
