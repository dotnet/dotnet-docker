ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.13-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.6

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='c4377df5b5b2e9d93e4a3c3b30bd42a17af86c1428e9a911a6e69a6441eca9f4163d05a9056cdeed0cf735819a6d01013b3ac35545f20f5a1fe87629cb3c3b18' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
