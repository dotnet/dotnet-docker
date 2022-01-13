ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22062.12/aspnetcore-runtime-7.0.0-alpha.1.22062.12-linux-arm64.tar.gz \
    && aspnetcore_sha512='1b5312b4216459c9e62ca45ee87f1b26ccfb244fa6119dcd74977da9c44877005e181f6b06b54bbeb41066f0586851c5c22cb2530a2730df3af0d7aadbffbab8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22062.12 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
