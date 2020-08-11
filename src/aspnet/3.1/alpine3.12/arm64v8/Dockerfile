ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.12-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.7 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='f986ef6b1efddf8c4dd2fff297bf596668bbaf3e7e43827b8c1a95cc1ac3236bbeb33ae467eaf1e2d0ab1914030c6f15bac4296c8ba2e185448272c4caadcee7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
