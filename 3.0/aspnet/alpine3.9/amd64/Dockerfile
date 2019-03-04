ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.9

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview3-19153-02

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='5f52ff31ad0c63a1e7c9af2c4bb9fce45acadf770a0000001481c13bf4fb7155f12a7ae7ae82497203f2ced6db6574234cd78489581628c01923973ff3b75a3f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
