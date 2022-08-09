ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.28-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.28 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='6034a6abe781e2a9d54caf00f407985db03c27d95df5f778dc9e9e530ad1a168e2ffcb279d9eab5053f24afb17f4c0d8dc25d6c73a1b68bbd6c8d4fbdfd4fe46' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
