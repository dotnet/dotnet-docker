ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.20-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.20 \
    && curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='20d23b0a50f51fa50650d7e51525d92f9cd7459edd6907e35c9a510004f7df8ca115e901a85822c1ef3252433293dfa80e9ac30231fcb99fbfa64a65b5d6df0f' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
