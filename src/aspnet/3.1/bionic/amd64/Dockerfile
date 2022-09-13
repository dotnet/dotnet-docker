ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.29-bionic

# Install ASP.NET Core
RUN aspnetcore_version=3.1.29 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='991918a89c1372d8d1eef967777cc9dd55d0cef827d940f068e701ca877dd6e14045c3a309e6e9c4a7f843eef6d1b192b9ae1257141947f999f4e8dd6b0d43e3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
