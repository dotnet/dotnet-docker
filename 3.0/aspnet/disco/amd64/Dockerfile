ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6-19271-01

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='c4bb96219ea776899cafe020630fc62724086200cd86943b0b4031ed6dfa81f27007386895c50596edaa1dc0422b35aa5a299613b82475ad85b0b220439781c6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
