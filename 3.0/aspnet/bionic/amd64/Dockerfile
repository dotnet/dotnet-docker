ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.1

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='f7f6c8e09a9faccb818c42312e77e2fd3d34a7186d611fbe9e77dfb8d02178f31a4f563a5ff8218678cd3c00e7e8e9aef10a69f32f44fdac9a616993c57d1253' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
