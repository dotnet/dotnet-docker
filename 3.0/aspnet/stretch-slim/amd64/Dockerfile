ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-stretch-slim

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview4-19207-02

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='bc491be399c174883a0b45d20ce30c298f7f4063c359c84ad37225e102951f3fad47f08025708839c4a9faf7b60aaa413e0c00baaab80fc405cb6cfc77fe09d4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
