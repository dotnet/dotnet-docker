ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview8.19381.3

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='c568a339060ca774e34a1dd2821c0b526357afc3a6b040a66c7b41e2ca92590f34494956761d551747c610d4961bd2b3322ae0f0ea9a168175d5bad7207c1fef' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
