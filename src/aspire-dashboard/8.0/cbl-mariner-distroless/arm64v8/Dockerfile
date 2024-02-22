ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=8.0.0-preview.4.24122.1 \
    && curl -fSL --output aspire_dashboard.zip https://dotnetbuilds.azureedge.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-arm64.zip \
    && aspire_dashboard_sha512='bc0c8d9426350e149f716f15ca49f542a2169337f6a2d03109a022db114deae6a8901984757c12c09eee2b506ab046924be8b50c55b7b4aae99c309630838842' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir -p /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:8.0.2-cbl-mariner2.0-distroless-arm64v8

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # Aspire Dashboard environment variables
    ASPNETCORE_URLS=http://0.0.0.0:18888 \
    DOTNET_DASHBOARD_OTLP_ENDPOINT_URL=http://0.0.0.0:18889  \
    # Server GC mode
    DOTNET_gcServer=1

ENTRYPOINT [ "dotnet", "/app/Aspire.Dashboard.dll" ]
