ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.18-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.18 \
    && curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='d26b9b637c33e1a64e0b3101c3e4059eeabb16096b1f558845d9d9d6fd3ffa01132a3210c2b90ec5283f899a174a38db036c60bb353e6296ee4e283332bba89f' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
