ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.2.21480.10/aspnetcore-runtime-6.0.0-rc.2.21480.10-linux-arm.tar.gz \
    && aspnetcore_sha512='28883aee0acf670b2099db7b5d69f74e9853ec0e02d2fafb3aee135f987a117d3a32eca04bb9213e4e39d7782d3cd37e25cfabfe6004269ac3eb20cc5d931cd1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.2-focal-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.2.21480.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
