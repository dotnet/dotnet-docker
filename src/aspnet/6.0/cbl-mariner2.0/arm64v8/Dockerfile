ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.10-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.10

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='a033694d1c4edf518bbc397080c2bc9947b45b20c05575a87660ed02bf457224dc2d5dcfbe01c3e6373219c7e3795e1e32d917de7f40163b86466df1e657e424' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
