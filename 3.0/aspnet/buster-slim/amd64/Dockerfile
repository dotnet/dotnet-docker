ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-buster-slim

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/3.0.0-rc2.19465.2/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='ac5be679851f7c874f1eb5cff1154f407346e3175883efecc533582a69fd4927005a9ec562c4b9e74b0daaadb7bb067f3256d89644bfb465b97d5b36f3563d84' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
