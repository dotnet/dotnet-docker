ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=17.0.0-preview.1.26504.2 \
    && curl --fail --show-error --location --output aspire_dashboard.zip https://ci.dot.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-arm64.zip \
    && aspire_dashboard_sha512='4b7cdee852f2d7f74a37f18f81a2f1af35b10ecd7bfdf6976b249cab2f8c8119b296d0845c927d61b90b6a710329b0f5a5c58d864f75d0a50fa4ba3c6a6dce73' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:8.0.31-azurelinux3.0-distroless-extra-arm64v8

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

ENTRYPOINT [ "dotnet", "/app/Aspire.Dashboard.dll" ]
