ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-alpha.1.20071.6 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='f9db614938f12bd17387bc9a62cf8448d9adbf65ea80c5bdcff0a712e0ce12a89ace9f71152b763f9264e1d5fd92bb321337817bb7e57d7ce1edf4ba7ebbe71f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
