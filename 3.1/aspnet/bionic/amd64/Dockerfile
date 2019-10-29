ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19528.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='f6109e54e71bc784524ea43f2b27b2659e095bfbd98c585b4ec593dc5e009052a27a2426370d00c80fb35d57f29a00094fb44e0415e74308a08cb88304f053cc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
