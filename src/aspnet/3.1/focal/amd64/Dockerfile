ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.8 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='dc332c14a6b0362becf8c8bbb1e3cc6a93bad1ea9c883602fdba35b53b3bf8078ede079c5918723add03ccebee46165d7f6d1ee4fdc1e04f29257a8155e33622' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
