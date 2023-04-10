ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.16-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.16

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='da61e20a735b450b0429b7bce90c5ab65254393f1d52e5f0f09363a718411d2a8e20321b8eed7c323f78b9f60c055b7cd2b2941343df7f89c10b4379bcb5cb59' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
