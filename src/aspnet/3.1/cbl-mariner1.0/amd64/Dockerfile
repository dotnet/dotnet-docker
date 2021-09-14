ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.19-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.19 \
    && curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='ec2788885aa4d18fa59bfa8ee62ee567fcc93c1708d9fd77c72778b862e6baca4a7519695ced2e362551fc11c66b0152fe5a30857537a7d3beacb052ff632cf7' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
