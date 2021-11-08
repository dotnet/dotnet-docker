ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.21-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.21 \
    && curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='483b0e7a82055b321047492f888d1c049ec800a3a834d206fd2bda881a393364b49bf749b5d98f1115c035f23fcf643eb744ef8d8710537c9a51b51686df0803' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
