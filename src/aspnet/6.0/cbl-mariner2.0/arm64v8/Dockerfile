ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.18-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.18

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='736993a1340de194259fe006f397e1bd8b5aa45029cba71818737b0e27bcfca0bedf742fd0422882af945933277bf52228d33dc57816064d8d412aeab5a1fd96' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
