ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19502.13

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='f59077c822af2308b3fd5e6705d75fe138cd521807523535d41a76daa96cb49cb66451cd92b0ec2b763bf6b1c1cb08725bf5635e586688c8740d22a45da4931b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
