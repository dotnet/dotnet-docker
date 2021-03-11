ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21160.13

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='36a780b8abc0fc5bbd8b3d5440fa1844265bbb3a948402d7a8b9f07654124afd9452549f431f048a8a7aee6b776dd0220ff578664a5449af1c40bc798cb5b532' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
