ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6-19273-01

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='12f3a0e9d4e724a30725aee4137be940f3ee33a5005b7d68cc95d58f769ed6f626d7b7141a217ef1b10b39b40af892f99956fa63656f106e568aecb08e5027d6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
