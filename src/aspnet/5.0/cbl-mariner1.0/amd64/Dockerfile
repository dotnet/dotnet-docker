ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.6

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='07fd5ab0b62e3d68a3099b76e1236550d86a1ec24150a09284d37b58055786595b182662671f0d4602545bfa404da8be0f9ab96036352dca28dbfef0048bb22d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
