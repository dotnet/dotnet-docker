ARG REPO=mcr.microsoft.com/dotnet/core/runtime
ARG ASPNET_VERSION=5.0.0-preview.5.20269.6

# Installer image
FROM buildpack-deps:buster-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='d231b80c27b23749914b7017544618a9078d4cd7f3ec274ef7b1acebd89fac1e4df8ede14db2af31d993cba940fc25e626c00e4c39e35d8512e9546923a69aae' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-buster-slim
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
