ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21161.14

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='1a6e5df2ddb28c541430faa6497fc91f5d72640daa59bd4d2d45b0fbd75c5835dfe02aacac439deae8c6b1d89a6cf81a877bb22fb1b8e41f351a6399be3bb7cd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
