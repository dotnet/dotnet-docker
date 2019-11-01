ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19528.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='b1d4a271dc9ca523fcd97e17890984705223be9953328b749539cd26d0dd7eeb1c8c15ecce2caf7443e37ef2d1d177d72bc096602e38c3d4c9c056a6d335be59' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
