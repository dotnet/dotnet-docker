ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.32-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.32 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='da6d3d12e9ea014fe786835d8e37ae9c8f33d3026dbf2c3fc95c011c6357dff9272bc5d62b9d929d38180dbe37ff7f9a9b051ffb72312591e5475a7ef4a42737' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
