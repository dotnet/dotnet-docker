ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.31-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.31 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='afd7493f9894ae5ce1d1242d7fd6ab72b13f1c35ca90459d56124904f85574514f16342a9833cc458253a4bde52d6dbf0181691addfb2e98a4d98d49f515d21c' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
