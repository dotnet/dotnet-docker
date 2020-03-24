ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic

# Install ASP.NET Core
RUN aspnetcore_version=3.1.3 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='ced0b7c9faaf250fbee2c850e8941d47a17da06fa8e576d57063fa4bbc6c8435de6ab488b683d4a692cd622e24d9592f11608a1b358074167cf0b76024815611' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
