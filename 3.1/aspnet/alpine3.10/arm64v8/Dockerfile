ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19506.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='178652af2f5aa51a9ce7cf795e17029d8effbf7506722a147832fb9e8c378fde278deec09020d7fae08c0944c1acd1994279ecd66bc32263e4156ac4c971ac2f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
