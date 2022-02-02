ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.1-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.1.22081.6

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='a729a7ec18d786042468696d72e07ef9869605d2bf5f7b160f1f39a67bb06ae33a423bbc733a69bca1a024321529c0e2c8d0095cb03d99e10c934b4ccb34c721' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
