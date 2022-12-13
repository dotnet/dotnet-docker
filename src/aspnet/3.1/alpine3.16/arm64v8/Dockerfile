ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.32-alpine3.16-arm64v8

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
RUN aspnetcore_version=3.1.32 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='3d4baee2ca9e064c6f53ba1d89c099273a245c0fa3f6406485b2d6ed0e0526df2e28d0f69ede1c5855d8eb6b2401c1d862b0a622e2ec7cc8ec671266d9563f2f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
