ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.31-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.31

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='c5c524c4167f06def7ff82b50dca46be856c1bd1c161ec905875420fc68f05cc3160d3a50beb82911eb2e4669e70f60930a439a888928e4dcfdc4432280e6ca1' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
