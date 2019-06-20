ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.9

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19319.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='fb691cae0081ea4ec6430bd7b7a9600d2678dd674bb604c5b21717678190c99a755855fe00e99f63cd3e82b7970db215b3e6f4b26905a4973a9bea9df977e755' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
