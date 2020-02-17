ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.11

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.1.20116.1 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='e9af60051f4141ba559999c71c9ce21a248ba356184797dbc203558647b6a18baa7f709967419567e17b6dc7faa3dc1e8c266d23ac7d5c9237023cbc79f3f4d3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
