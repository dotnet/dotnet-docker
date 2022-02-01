ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.1-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.1.22071.6

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='ab76edde0d1fe2965954886ac50d942470f9ab35521b1619c7a23801a67853aafe2d178500e8314645da976a3fdb175196b12fddd6d7de2a931b79e07e5990dd' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
