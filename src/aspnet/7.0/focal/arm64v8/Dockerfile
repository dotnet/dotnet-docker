ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-preview.2.22074.8/aspnetcore-runtime-7.0.0-preview.2.22074.8-linux-arm64.tar.gz \
    && aspnetcore_sha512='14c95482568633ce4152457c642edbe0ed2aaef019aaaf17d83d123ac7235b3f088a3c36194a9c89f2e5dbe105d70cacde990d009b230c26b7019f1b6502d8ff' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.2-focal-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-preview.2.22074.8 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
