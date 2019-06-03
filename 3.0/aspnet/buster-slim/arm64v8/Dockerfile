ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-buster-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6.19301.2

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='f8cb5d9ad4ae227411d731b1a0878b1bf6af87b5a9a7a53b456f8e2e118aaf00f7acce8c2db58b69f1603804f329c9b8bbb808ee2e46b82e422ff0b68cdafe6d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
