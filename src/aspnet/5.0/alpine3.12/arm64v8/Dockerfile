ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rtm.20520.6

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='ab4dee6fea790a93c9b67cc28d7f07bd9ff45fe64fad4089ad308432ce1e8857975415dfe9e07329468b32acb0d867289e3fb68bf01ce958bb92729bcd6373ec' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
