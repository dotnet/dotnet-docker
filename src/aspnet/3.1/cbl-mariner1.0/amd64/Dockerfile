ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.24-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.24 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='dc0f82fa2ceda9f380b1a11514b624a5bb77e70d3eaf8460e6b2ccd3205b8605e2cb5921d5db42a6f0b7e4d2fbb4a85a09b4dc7b95909e934a11b1b2840cec25' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
