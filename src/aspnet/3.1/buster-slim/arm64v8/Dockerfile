ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.32-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.32 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='34b9ec241cd0047cb23f0b8416d3a009476e511c3dd5854636c11cfd078117faf095f32f06e7c97d810af94fde43621117414f983d3b2041ad40260f50dc330d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
