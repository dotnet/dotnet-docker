ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.7-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.7.22376.6

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='a38d37c589811909cdf6fecb435f6a9e54582d1e88ea3ca1e6a6812e742741a33b7cf7aa373c664ab49518a9d859f09615d8e837c69b32bc3d9d01aba1596b84' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
