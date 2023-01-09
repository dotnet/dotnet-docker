ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.13-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.13

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='6a9cb2e5504a0954870e36e4ac9285e383c4714acbe6ec7f2c5197aa1f8bacef4b659103493e532ca46001e71c8dc3c90fdeb6cc9d2bd097aadc938790f7297b' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
