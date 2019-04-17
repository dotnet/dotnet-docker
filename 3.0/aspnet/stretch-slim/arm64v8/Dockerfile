ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-stretch-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview4-19216-03

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='d6cf82216260f1fd4cd4d58f5991685af8d103e2a1b084cbc1ba8044eae77f2f60158db58d0daf223d9f17207eed7bb0c628e53dccbe8e29e627eaea759a6ad9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
