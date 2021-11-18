ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/7.0.0-alpha.1.21567.15/aspnetcore-runtime-7.0.0-alpha.1.21567.15-linux-x64.tar.gz \
    && aspnetcore_sha512='89fa917fa391f96412ae7691749208d185cc26c9e3e5aadf23950a10de9dc9d8e1af0cd03a3640a2f7050d74925dab002e31b1b6e3bc7ac48a4afc5120db6574' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-focal-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.21567.15 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
