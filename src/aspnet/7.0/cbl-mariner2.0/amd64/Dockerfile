ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-rc.1-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-rc.1.22415.4

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='285d7c919cdec81213346873afdd1f866dfdd218790305d8c9b00ba300178b805d37baaab593276c2aaa042d8de08d7e680dfcd2be4592f6b6a56d1d66b22c4c' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
