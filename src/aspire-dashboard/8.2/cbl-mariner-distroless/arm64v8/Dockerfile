ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=8.2.0-preview.1.24454.3 \
    && curl -fSL --output aspire_dashboard.zip https://dotnetbuilds.azureedge.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-arm64.zip \
    && aspire_dashboard_sha512='5fa4442132466d7c7ca445ad8444c4a59298310fe8c6a155a86460bfbcd1e8b9cfd8d8ea1e31447b1b3bfe1f6db1249ec9bb294a06a0c4576ca1ce4940cf56e0' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir -p /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:8.0.8-cbl-mariner2.0-distroless-extra-arm64v8

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # Aspire Dashboard environment variables
    ASPNETCORE_URLS=http://0.0.0.0:18888 \
    DOTNET_DASHBOARD_OTLP_ENDPOINT_URL=http://0.0.0.0:18889 \
    DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL=http://0.0.0.0:18890

ENTRYPOINT [ "dotnet", "/app/Aspire.Dashboard.dll" ]
