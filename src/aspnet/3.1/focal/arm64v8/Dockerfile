ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.24-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.24 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='ca3c4536cb8b4464f11fc528744fc1c0c972f5e743b0e05c6da965551f64c521ee8110fa90f3346a5253071718342144abf2cdaa9012482ce1b4176a48a26478' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
