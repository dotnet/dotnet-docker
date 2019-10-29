ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19528.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='218d7a70dfb630bcba4614d13e2bab567754c4fddbffb59fe842b2985c4fcc0f4078802b7b174a364253c65588e7a8f6bc17f433989974cc55baf455cad4f4b9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
