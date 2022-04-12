ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.24-bionic

# Install ASP.NET Core
RUN aspnetcore_version=3.1.24 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='49e4796e44fccc5ee8ec6b5506a1f2d0a0de123b152ac3f9320823699a0e1f8895d83f13ab8f7273863ec64274a06027ec7f8896666b9c43b1f0d3ec3b3ef81a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
