ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.32-buster-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.32 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='0aa2aceda3d0b9f6bf02456d4e4b917c221c4f18eff30c8b6418e7514681baa9bb9ccc6b8c78949a92664922db4fb2b2a0dac9da11f775aaef618d9c491bb319' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
