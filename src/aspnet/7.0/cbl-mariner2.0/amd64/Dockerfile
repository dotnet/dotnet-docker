ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22273.7

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='fe3c5dd14af13bfe0deb26c6b642402fff4d5984d0542d7d61f12fcda6056436e1a4f3948971d9980a835a0dd35bd80c59c455ab787d4727ac17916c521e390f' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y aspnetcore.rpm \
    && rm aspnetcore.rpm
