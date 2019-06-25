ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19324.5

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='893bff12197f922cc7f06c2af161a8bd32cf339bf084ec5929d457a772faec13770816215458d6b5350376515bc9be4c6cb41974058681ec0f6b7b6cddb7551c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
