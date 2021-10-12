ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.2.21480.10/aspnetcore-runtime-6.0.0-rc.2.21480.10-linux-x64.tar.gz \
    && aspnetcore_sha512='d48895237644a3999663b2e16baa2303d8b77f66385a04d8edb5601fdffacce18b1b1318827d09f74f6b133ff2c179ef659bd21cf8460289b5f81f404fa8b326' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.2-bullseye-slim-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.2.21480.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
