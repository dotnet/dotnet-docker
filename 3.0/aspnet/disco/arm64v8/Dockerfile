ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19353.9

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='001a1e4a56516f99844f686807a1a39648225a540ff9763aaaa32487723589d33dd1e1f2509257fb708d82d801f129bade8d25bf1c9109a6e59319a5ac69296a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
