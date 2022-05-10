ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.25-buster-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.25 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='6b0d39062374d5574cd5b52bbce033f7769f231494f9ff93ba6f1368195742eb1796f25268fee8a2f44ed34a44d822d11b8ff8de0ba06515e75b88c3cfbd0244' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
