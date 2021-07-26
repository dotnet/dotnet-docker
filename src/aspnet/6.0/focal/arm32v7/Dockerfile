ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-preview.7.21374.2/aspnetcore-runtime-6.0.0-preview.7.21374.2-linux-arm.tar.gz \
    && aspnetcore_sha512='ff314ecc15311099a649c4eeaf069d14316f25f149a9c879d6554eee76a94d64ae87286de27a9441524cbd8ea7a74b1a7dd3bb508a59755f85e18e1f56317112' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-preview.7-focal-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21374.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
