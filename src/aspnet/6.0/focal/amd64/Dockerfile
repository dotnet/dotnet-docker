ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.1/aspnetcore-runtime-6.0.1-linux-x64.tar.gz \
    && aspnetcore_sha512='9e42c4ac282d3ed099203b9a8a06b4f1baf1267b4d51c9d505ca7127930534b60d4e94022036719133b30c1b503f66d7d4571bc24059d735e510f5e455ec6c51' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.1-focal-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.1 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
