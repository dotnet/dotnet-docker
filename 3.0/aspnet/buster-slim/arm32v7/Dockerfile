ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-buster-slim-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview8.19381.3

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='832c5e2e87906376764e7639adfc387be7d8f13f4b87beed5f0afacadf5a7174a3377f90cf11ead357298bee798baed4768d0e09a5bea38d5524e17a068adc0a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
