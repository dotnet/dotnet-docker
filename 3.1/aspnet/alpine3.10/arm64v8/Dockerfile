ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview3.19530.9

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='3f9c5dc3a52d29f69351fdfbce79325f5da19879bf6650cfa705d41e848a8001a57d3a5770a463a7d8b772d2856f31d54e1a20c4d32c6a494d60420d8253be58' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
