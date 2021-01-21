ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.21069.12

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='749ad8d7322358d21e42a8cf902c2164871263ff1256098bea0e8ea81de11d97b88e4de6a5f44bfd429f63b96ef88568c4450131282c21c0e2f324c80619dae8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
