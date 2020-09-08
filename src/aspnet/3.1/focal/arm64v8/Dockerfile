ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.8 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='80e05a22b4c82599b4b25c7943a124b68db3e70f7d0757d64bda6b4b46bed7b54813caa029ab16e515678d692f225eadef35a70435c1384f5b8e2f96c7cf3edc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
