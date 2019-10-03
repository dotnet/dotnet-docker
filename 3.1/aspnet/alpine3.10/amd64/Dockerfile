ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19502.13

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='35a6e7964f06c7e5a84b91c500d566ec3bb2490cae2a7eded02b7b6fc926c1ef5484517c3df5afbdf68bf48edde5d7d13480cd05efd207b70a59f0c3f8fa35f8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
