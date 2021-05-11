ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-alpine3.13-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.15 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='c8607c614ad241c8d60060680acf588d2b5cd0261d13904374202521e822cf2ea81f1f0b500295f596755e04c4101aee5f71f4c71405c9505fc5f26d8daab346' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
