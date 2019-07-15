ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19363.1

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='d6508ed125407fb678c0479fae888503b3553dcb4eb006871b65eeb9a6e6e21ac079c7e36b47dc124fa60eeb36110a230d39e29453393235dbebc7b558d46ca3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
