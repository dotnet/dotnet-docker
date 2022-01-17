ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22066.1/aspnetcore-runtime-7.0.0-alpha.1.22066.1-linux-arm.tar.gz \
    && aspnetcore_sha512='94c3645809a74f94a7f03dedba29459282da15b31c3d80d1d8d7a72c63931ab086b045eaa6a8a0aff8d4ee05b41f0b48ea06a517928e111a70d7971f8f8d8242' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22066.1 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
