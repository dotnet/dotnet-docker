ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-alpine3.12-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.10 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='d6aa43dade937606af8301e758843178954383807b4e4425592a0b8e1fd0c07051862b6939de9c5ce263e6c73076070e662784107e85cce3fecf02333738ebab' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
