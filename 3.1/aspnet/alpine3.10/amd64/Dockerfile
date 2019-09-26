ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19508.20

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='94f38dbe84a95b444371196ee88b8e3cd7b3885bb696c950a06f4b15d830e99ff22137e747ce1509bb3049a038e85b8a962274b3ce7c35a8518b4236cbab013b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
