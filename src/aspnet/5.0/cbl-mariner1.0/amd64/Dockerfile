ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.8-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='77c9854f2bc947675025a9489baec2587cc9ef95e3be9351060c0d64fe0cede3e9c1395fe700715df520f106dccda9a21f7d21981df70653c49fd20418dc385f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
