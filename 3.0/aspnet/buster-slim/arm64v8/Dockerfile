ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-buster-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19351.3

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='74cd8c9a15bbccb15815d07b0c7602710b182a14aea7e1970a45808068b25e873b3d7d9b542d45a51f1fc3dee3e58332955fe24fe899c05ac1ac39f31c0a6a53' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
