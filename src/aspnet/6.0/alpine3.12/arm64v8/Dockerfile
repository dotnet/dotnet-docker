ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 6.0.0-alpha.1.21057.7

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='07a360b548e9c5f1666c63aacb5ce39f70df00a1a51978fb1c18ad552cf453107f764fa17b664dbf4d23770f77e5788224d3ccbd944c397d958c85afcfc32f67' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
