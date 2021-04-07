ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21201.13

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='80d451713218d1d0ce89d5d0e5205d82811e0a58aac299403cb7fd305f5f70acbe3b21854d2ff85d7666a48a083571c7ea661c7499f8900cac6571732876f831' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
