ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22063.25/aspnetcore-runtime-7.0.0-alpha.1.22063.25-linux-arm64.tar.gz \
    && aspnetcore_sha512='f6338d33e1eb76bd1f47d1cbdb767c6a259bbefac550d7488add24cbb48a9dcc1a8689c9e80b105a8e64c4f7b2474ef3ae3babf4f77b3608db42fbc7792fab1a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22063.25 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
