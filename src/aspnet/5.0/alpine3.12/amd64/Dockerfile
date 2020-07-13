ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.12

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.7.20360.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='1cfccebe84efe2282b112741a3fd9b7a04528da19e278bb0db464fd98c4e37c89567b1c388c1101c80d27a8f936c9d0f7571cee2b88014e05f6f97aa665ae3b5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
