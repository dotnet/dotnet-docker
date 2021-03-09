ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.13 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='05aaa12d951e7d0fae14ca0f7fc20ec921fd70cb016abd043663565282ce112f8d37f6a98e79c7b4449575b604ec06e73ce01fc349ea9563cffc37b606d7d239' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
