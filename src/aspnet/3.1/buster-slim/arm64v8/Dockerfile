ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.25-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.25 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='003d0294aceaaa51adc2dd0d22fa645eba612e33c70504979b152f16f8fde8d0d07cee9056c5871a5773269f9724840f1618828620e1a31ae9b7ff6117c0336d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
