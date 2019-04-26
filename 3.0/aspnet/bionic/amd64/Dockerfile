ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview5-19225-01

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='c4bed085fe06f808a5ab6904d7be89c031be8d4cd60b67d55c84d3ebb32adcd9e04cb1a7ef102ce71243fc3cba26749807a49fb96345e14acd1753d2b272cb10' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
