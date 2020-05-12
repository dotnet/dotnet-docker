ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.4 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='6fb96f99639e2d0105237dbdd359c6aa59608ffddb2fd257b1932a214cde4f5fe34fe3b46dc65cc0e2f1670f4c1fb08fde2ca0b93bc11406357d4f40531d66f6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
