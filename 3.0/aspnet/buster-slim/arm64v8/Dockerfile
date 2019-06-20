ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-buster-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19319.3

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='39efac739609ed2e421c8032bae01132958146eba433705659928f70fc5d2eeda7f5496de33cedba750e55ff50bd1cbf0c4a9faa659cfc058f5bbf57ec31493c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
