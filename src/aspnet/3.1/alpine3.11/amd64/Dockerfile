ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.11

# Install ASP.NET Core
RUN aspnetcore_version=3.1.5 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='b56aaefdd188106b47f3c20aa65f1fb3b9bdaca450e9599f132c178803119e7611ff8cde07c9248a49923dec1255d1e3e2fb53d7ec5807903c7bd1b1b9954a88' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
