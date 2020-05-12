ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.3 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='1675fc5c801b0655e752aa91c132d6e4103f00ecc9b7f18fde378812d323462dbcf74f3d48ab63f863197b7aee33726286a37c3c5ed360f75278dc9ca677f035' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
