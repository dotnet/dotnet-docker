ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19475.24

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='c61b5c49fa4284843f6b03138b01859e30138c14c52960783d1e4f6e5c3838ec7bf946e33394843e478bfbe9d0d627a9e5e0aa0ca1abc2d7ffb7584d2d8efede' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
