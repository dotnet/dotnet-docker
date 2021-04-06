ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.13-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.5

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='04057353d890e73f5fe93cf9d05b2e84bf1f972a401acc631fc7ee7b83e97a4e40343458f274b7e900f96b94fbd2b954bde89b8874367776c82cb17567091d23' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
