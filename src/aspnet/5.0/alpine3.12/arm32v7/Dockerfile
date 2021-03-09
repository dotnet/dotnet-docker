ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.4

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='19645e9e429af96fdfcf378b2fd070de3ff9cb91c1b0ba9fb7b20afc11ac5645ae2ec2388d6a0ead9b37a58e20dd18cbf46b76c1e0e3f5358c59c1297fc83dea' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
