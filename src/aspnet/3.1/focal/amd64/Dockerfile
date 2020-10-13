ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.9 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='86462c61dd71adda38ddb0178fc44591cde13de4357652365e0d5c80d14db98d2e1f14a6fab2455b9deebcb910577174473d86f432dd3cd3d0b4284a9dcf440f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
