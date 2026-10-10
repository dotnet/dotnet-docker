ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=13.6.1-preview.1.26506.6 \
    && curl --fail --show-error --location --output aspire_dashboard.zip https://ci.dot.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-arm64.zip \
    && aspire_dashboard_sha512='fc72aa3cf8b8f225537e29dd6856d8787713943997703418e4cfa9fa084e2720d17836bc1b1f67bf37efa451f2ebcb8eca27908aa8c31a13a3182875a71020d6' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:11.0.0-rc.1-azurelinux3.0-distroless-extra-arm64v8

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # Aspire Dashboard environment variables
    ASPNETCORE_URLS=http://+:18888 \
    DOTNET_DASHBOARD_OTLP_ENDPOINT_URL=http://+:18889 \
    DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL=http://+:18890 \
    DOTNET_DASHBOARD_MCP_ENDPOINT_URL=http://+:18891

ENTRYPOINT [ "/app/Aspire.Dashboard" ]
