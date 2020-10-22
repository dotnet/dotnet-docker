ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rtm.20515.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='34d17a434e8429a0b499cd1508f07acfbf0d5c97368c766b2e75af1cf313b7d1ac3dc41033e930e1ee0a01bd91e182d7adabcbd35b7de3c3182728a6769d6ee3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
