ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.29-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.29 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='5066d2eb833b1f6b00c2381c51c855d82d9b22c1487af67ac40d4035cc1b90fcd06b9bc3b3cd801e116c549942f8c02ecacad4b623aff5c4eddbfb57c96417ef' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
