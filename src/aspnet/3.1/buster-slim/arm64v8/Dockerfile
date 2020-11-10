ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.10 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='b23585cebda43ec01d866013519c64456733730a7d74593b0583bc54327321e17dedeb3c8127097edceec105e979909db7097ad53ee8ca9dfc434da9007588e0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
