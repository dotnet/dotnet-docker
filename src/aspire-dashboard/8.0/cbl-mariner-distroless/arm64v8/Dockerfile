ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=8.0.0-preview.3.24075.10 \
    && curl -fSL --output dotnet-aspire.tar.gz https://dotnetbuilds.azureedge.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-arm64.zip \
    && mkdir -p /app \
    && unzip dotnet-aspire.tar.gz -d /app \
    && rm dotnet-aspire.tar.gz


# Aspire Dashboard image
FROM $REPO:8.0.1-cbl-mariner2.0-distroless-arm64v8

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # Aspire Dashboard environment variables
    ASPNETCORE_URLS=http://*:18888 \
    DOTNET_DASHBOARD_OTLP_ENDPOINT_URL=http://*:18889  \
    # Server GC mode
    DOTNET_gcServer=1

ENTRYPOINT [ "dotnet", "/app/Aspire.Dashboard.dll" ]
