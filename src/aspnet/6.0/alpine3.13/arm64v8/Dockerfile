ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21179.12

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='e9413588d5ef52c4fdc01d9f8829cf6d28935ac0bd5306689b9595fd00b4905833e3aa7a1cac5d6313ccb0688e59fe990815a6389821c701a0e31723d787c6f8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
