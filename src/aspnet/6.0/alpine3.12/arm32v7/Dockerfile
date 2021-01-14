ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.20629.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='9266bf5a1ad8310a3a32f6a43dfee2bad2e4feb87f5f4c78eda207bb460119032f4a1dcf48f8944cd63c6ccf502a14bdc9db72c94d6027145f86ac38c0f7e87f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
