ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.6-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.6.22330.3

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='5d5314065d218b82a8253b6c4cea292d19295cd8242cf2f7efcea95f984703caf2f280023bcd73c700d46d2defd95c02f4e6febeb46719b98f464dc5fd40c2d7' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
