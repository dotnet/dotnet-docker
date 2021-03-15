ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21164.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='991865a5d2921fc6ced9a915b7f72feda7edd8135cfbcbc9a1c945e899b9ad50efe1190364ea26c82162973626bae646ba112b83ed2412b7fb21be767424c4d0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
