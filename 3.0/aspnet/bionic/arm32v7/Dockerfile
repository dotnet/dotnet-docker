ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview9.19422.5

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='e2b0ccb94325248865c2c24ab23284b6631932d31a1edae39e4d9bf08991d2ff5a19cc9a391f46f0c962cc99b5fa0d5d22985c21c7154f28a58a49f7ebf29a76' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
