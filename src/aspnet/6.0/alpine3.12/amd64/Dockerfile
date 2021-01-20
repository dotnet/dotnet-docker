ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.21069.12

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='d855cf8a86996dcf750bc1fe7dead2f7454ab1909ba1721e03d25e8360e35430d903c8cd87e0fab6b3682f1fd9794abfcd2a07d6a3a53e2f52e261bb4590c353' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
