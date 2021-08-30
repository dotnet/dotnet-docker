ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.1.21427.16/aspnetcore-runtime-6.0.0-rc.1.21427.16-linux-arm64.tar.gz \
    && aspnetcore_sha512='a51cb7aaf12f6e76a12def2fc47319671fb464139f739a80fcdc352c40281a19a71bc9e2e52f7a96e2954fde40f8baac38be735bcca5483fc971ba0e05c89acd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.1-focal-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21427.16 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
