ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.12

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-preview.6.20310.9

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='5c9738fe3ea8fbcbcad0b0ec42b60e436dbc739d49b811b720208d7acca3c3cf14ebfc8b00dd8b27d57f8a95519d3951983343899df6549d861393cf183aba8a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
