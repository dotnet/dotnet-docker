ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.23-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.23

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='66d587fd2181cb2246a4493b821649ad9d1956065f102a77ee0977eef4784976e86d47cd3457e978fcd9a50d436d1ab815d562d487b03359e1c464947d2f56b3' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
