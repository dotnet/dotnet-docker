ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22303.8

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='983d07834d15915199e2dc07f0540eee3e7bc7b9f5821e39702a63438f8aa67f152c334141b3525c52d5a665a6fb40bf466d19e480b46d4e85723bd7e66893d8' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
