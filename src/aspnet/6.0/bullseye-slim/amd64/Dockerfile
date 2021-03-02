ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=6.0.0-preview.2.21125.25

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='45045b98a6585ff9a6a5370b2072e1233eed6c4b0e34c06a00776f7962c7eefd03f5b5e9a9ed682e09531adee8f513799e0cd134f6f9927f00cade0128620502' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0-bullseye-slim-amd64
ARG ASPNET_VERSION

ENV ASPNET_VERSION=$ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
