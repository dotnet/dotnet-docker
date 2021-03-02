ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=6.0.0-preview.2.21125.25

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='5b2828f579b2d7c476a013390a8cab1eaa3b59ce1db7b666ed311a899a53be1366eec6a4caf285e8684303dc18c68196d26bc09e01d7f66e582f17965e42c567' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0-focal-arm32v7
ARG ASPNET_VERSION

ENV ASPNET_VERSION=$ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
