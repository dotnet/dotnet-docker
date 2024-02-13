ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.27-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.27

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='cfd1172cbc786fce60387f29fd87cd744e950da43097af5e9ebac776c95e5ed0c7b7117ca83bb65319a285410a9df81a35d480bd8b7743cab51b2efc0fd52b04' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
