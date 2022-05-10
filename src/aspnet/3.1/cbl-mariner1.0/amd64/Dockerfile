ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.25-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.25 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='04adb799ebec0b6cafaa04c9eb00adc20dd0811676a9b1512432def1c816d2303c7de3df00b910106eb5b05a4d1b2eb461d44242d772b0a95a04cfa4f8d2b3a7' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
