ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.7

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='03399412dfa6ec9b75125b38d7d4297e0cb8cb4ae37cfb27d6cc948942763691c61647d83c3a8a068291a389c35fc6a89d375c97253335c6a8985d55fcac8918' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
