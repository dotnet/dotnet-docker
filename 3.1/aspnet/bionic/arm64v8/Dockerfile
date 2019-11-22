ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.0 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='0a7fae3a4549b3954037adeafe0673728c7b4e765aa0a3952e58de2b6f746d3785434fe2970f6a4ff78c9228e2ffbda2e400e47917ef9da4dc938f4ef09e2643' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
