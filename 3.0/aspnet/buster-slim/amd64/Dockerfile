ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-buster-slim

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview5-19227-01

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='7c8790c263806dbf7cffef73fc39c52c46dc9ce7f43a3a290ca21b271a6164a57abee4619d021c04097c43af4dec0e2be205cf0c12b4ac498290cfca77ec1de5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
