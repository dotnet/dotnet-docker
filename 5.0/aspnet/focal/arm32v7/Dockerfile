ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-alpha.1.20071.6 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='987244cfbdb560347c231797e573b0f878dbdf632ca216e85d91b25f5e18177c3cad53e59c0efdc3c9dff8bdd33c7eca5066de966f8d65061424fdcd583ef994' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
