ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview4-19207-02

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='3f3733999ceb2b21b8392248572b9dfd0c8192c578633315a13f7fba5b5c79706a1093d98ffc110d469b2968134ddfc7c1468768c7d93cb211b89355e12358b8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
