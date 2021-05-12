ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.13-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.6

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='df8d86e72b40fa7edde859984a882b6e718bc8dc307cb937c6c2cdfb21a18c8a0cc75c02c2054a2a62aefd3cdf96c183a8cac5adc8d5eb2f2fb82866db199e57' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
