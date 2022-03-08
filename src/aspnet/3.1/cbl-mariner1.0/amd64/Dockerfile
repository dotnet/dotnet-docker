ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.23-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.23 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='1e7bbf255316b22378921cdd606ef653ff1ca9c1108dd8efb29f8c2430a4c3d1bca58d66c563a83342d78b0068a79feb0efce5aab0a5188db8a1cdd5b52087b6' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
