ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22303.8

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='5d76eb32c33f233a3aca22ba853b9c254ece59e6f6c726475a6f4dca1d61f10e1eb304f7d7ec067cf58a6909bb5e63e573a44cb85531f094141486472ee0070d' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y aspnetcore.rpm \
    && rm aspnetcore.rpm
