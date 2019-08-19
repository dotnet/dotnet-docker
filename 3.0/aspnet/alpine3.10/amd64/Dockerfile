ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.10

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview9.19416.11

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='ed6e3f4dc7d670919f75520b48836eaf758d4eb5dc1e73220e04d31d5745cc09df9a4b0b6eb3980ae09a68fdf337a2edebeab1bacff944995cac7f7fef8220cc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
