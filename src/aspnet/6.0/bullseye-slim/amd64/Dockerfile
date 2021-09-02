ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.1.21451.36/aspnetcore-runtime-6.0.0-rc.1.21451.36-linux-x64.tar.gz \
    && aspnetcore_sha512='b89dd6564ee11fe4e2e0a1c94526a5fd2742c93381935050eb64f0726e2d623fdf590e0bdd6ba6a1688ab26c767b26ea15650c764ce21369680a7d5ccf4c5d2e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.1-bullseye-slim-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21451.36 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
