ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22273.7

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='f8785ded1cd44b19bd37f86ff5718df306dbceeba2491670cdaed46f5fff2a4f37adc2d460c4ea5d9c8207815808e627ac259998f3c6b826759dd0ee194e4fb3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
