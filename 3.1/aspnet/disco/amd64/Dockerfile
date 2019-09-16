ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-disco

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19473.5

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='b364d2645ce71795ddcb789716adbb1c44db7b62a3845e06ed5b0cbdff3db6b751b342b05f79666a7908de9ec9ccc95c07b7811be3ea01d18e0b0c1ea39dbf83' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
