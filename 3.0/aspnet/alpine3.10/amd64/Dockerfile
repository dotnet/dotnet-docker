ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.10

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview9.19422.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='eee6764e44c2faa7e0b55e9f10c17b3f4ebd403793b1162e229f32ffd3659d7d95e5fa47a13aae9950263d716dcc99fbeef6ac18e3fdbcad8263b237833a72be' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
