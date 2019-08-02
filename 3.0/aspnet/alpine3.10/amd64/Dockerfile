ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.10

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview8.19381.6

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='fd7fe5c915bc34b6b79033fa3b51e62ad1e3788914d01849861702a3fc25c7ef1f2a6ffdf8d249999252434d5a5492f8dddfe4fcef8a6df3c0064bd2dfe756f0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
