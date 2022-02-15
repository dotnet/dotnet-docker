ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.2-cbl-mariner2.0-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='00ecdb4343d6f42861157aaef7d892bb3d0e0c0ccd8035305708aaf6bf7095fd6daea2e80cf4fa773aad063d6e510e70cd84d6c1efbd2e3555af6cda0f4b5d31' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
