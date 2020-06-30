ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.12

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.7.20329.4

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='1fc9fe7ad303fe119b3ba4a6e853d7cb05d373831a5adbe62b9acad09ef0d5936a1629b3f5138799f497d571c597bdc7fa59056b6dedd21bd076ae63af2cb299' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
