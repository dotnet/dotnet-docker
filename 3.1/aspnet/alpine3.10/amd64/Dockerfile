ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19503.31

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='9f48a0b2d24f6b73fb69796146dac6295724cecf206b7b8039689592e710aa700ed3c5fb00708b8792f33b638de19709bef9690b846aa6e4781070ae713f5225' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
