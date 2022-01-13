ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22057.17/aspnetcore-runtime-7.0.0-alpha.1.22057.17-linux-arm64.tar.gz \
    && aspnetcore_sha512='80cabeef18bb5a868c5e7abdace3c40f1db41e4c3f13cf8298d70980237c17f63487928471420219fca8dde6c1f37dbdbe247d2fc94c2fe566339b5628648612' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-focal-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22057.17 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
