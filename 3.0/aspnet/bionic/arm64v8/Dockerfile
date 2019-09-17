ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-rc2.19464.5

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='0ea0ca5c74b3178e0de182b81693db0fecd8e4f7a99f7e46cff330221ac3c71c83ca4ea50f6d97300462d04ae24d8714b4e009413f52d245b000b22dc488303c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
