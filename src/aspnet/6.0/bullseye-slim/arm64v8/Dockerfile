ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=6.0.0-preview.3.21164.2

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='c5ef6e69a0f02dd33236f9c50716c49e27266f532d3298bc32d4bcad6d03a3059e5c98c59ba8b24e918eb29aad70e49638a3367f33efe82a8be9882d1809954a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0-bullseye-slim-arm64v8
ARG ASPNET_VERSION

ENV ASPNET_VERSION=$ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
