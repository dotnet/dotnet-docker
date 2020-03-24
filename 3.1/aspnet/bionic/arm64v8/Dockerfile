ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.3 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='d336cfb37c93eeb7b8967a55b5b324f6e0bac78c7e8b70809d9a589cd8b7101f1c940bcbf046b55df6a92f0c1d53f2c70f543010dc4cccd9e5dd310e838631a2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
