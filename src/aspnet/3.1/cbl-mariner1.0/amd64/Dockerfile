ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.30-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.30 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='ae3e45c82b6f46ba304c98026055c8ef0e146426a47f9f03a50493d8715df12c79f044ae4f022dd4af9abfd1a348f9a54f220bd6c1e42804afa802a98f9e61ec' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
