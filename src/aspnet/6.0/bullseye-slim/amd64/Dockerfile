ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=6.0.0-rc.1.21365.18

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='d26ae4b5bc3629fefa4bd0c96dea503a8ac9963ed96be3639fff6d1d83a98cc1d72a44ba6a782e3750186a1c4b9e6dca61cfb7f6510e4947e2cc3ee140f3c547' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0-bullseye-slim-amd64
ARG ASPNET_VERSION

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=$ASPNET_VERSION \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
