ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=9.3.0-preview.1.25265.20 \
    && curl -fSL --output aspire_dashboard.zip https://ci.dot.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-arm64.zip \
    && mkdir -p /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:8.0.16-azurelinux3.0-distroless-extra-arm64v8

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # Aspire Dashboard environment variables
    ASPNETCORE_URLS=http://+:18888 \
    DOTNET_DASHBOARD_OTLP_ENDPOINT_URL=http://+:18889 \
    DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL=http://+:18890

ENTRYPOINT [ "dotnet", "/app/Aspire.Dashboard.dll" ]
