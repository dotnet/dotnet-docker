ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19475.24

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='8980636b9a57c394806170f07f0c24bcd791579fc7588e8b0f886412859008c188e698d4d4746f53707a5e84352bea417bf0324f1ee2d52e48b31f158a503966' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
