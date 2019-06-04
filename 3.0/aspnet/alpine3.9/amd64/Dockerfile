ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.9

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6.19303.1

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='901fa39291ba82b588cea32a5dc46a41f2cb157b1f8d36e4b4e37032fe315da96ff16a64d795bcb7af781df59b0ed6c12259cd8adafc9daaa951d10bf13bb1c9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
