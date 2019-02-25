ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-stretch-slim-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview3-19122-14

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='00184f3dc314928403e1f0c9800cbfac09b2129c76723a49feff41c70a62c34757672fd8df858faf78ceaf37097bba95c3cb5d967be5ff3ec202860dd5f562d3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
