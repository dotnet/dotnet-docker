ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.4-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.4

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='18ffa72b38dcd01bbfd9f656996e994dbcdb7b6b196771fc498bbaea774ad844f7fd5418487d0a5a7f83a76b3683f8913e22275bc25d66ee0c0d84ea4e279971' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
