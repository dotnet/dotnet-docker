ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rtm.20517.7

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='8d826bdceac5d4ebd6c56656da6b62450626bf9eef3d20a2cb6ed4ed859c5c15eec4aae7027d869c1d3ed8321b592a826dd1882233578947aa35772f76b16c86' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
