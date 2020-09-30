ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rc.2.20475.17

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='35e85d3e822317a96dbba0aa239fe05e4f85e77031e3ba0c1fbd2a0800898bdaf377b800ab2a02510d2c0e0e6c51da1659b5afa919ebd0540fea2e02a8d7fa92' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
