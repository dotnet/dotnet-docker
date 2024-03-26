ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=8.0.0-preview.5.24172.8 \
    && curl -fSL --output aspire_dashboard.zip https://dotnetbuilds.azureedge.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-x64.zip \
    && aspire_dashboard_sha512='3bfb8d6b97a48d0265674fa5f3acd2bc9871fbf2ea7389eee9b4d5cc58392d28c210fbb149e6509081112267d40208d8e5b8eb3729996b6aead37895a5d2198b' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir -p /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:8.0.3-cbl-mariner2.0-distroless-extra-amd64

WORKDIR /app
COPY --from=installer /app .

ENV \
    # Unset ASPNETCORE_HTTP_PORTS from base image
    ASPNETCORE_HTTP_PORTS= \
    # Aspire Dashboard environment variables
    ASPNETCORE_URLS=http://0.0.0.0:18888 \
    DOTNET_DASHBOARD_OTLP_ENDPOINT_URL=http://0.0.0.0:18889

ENTRYPOINT [ "dotnet", "/app/Aspire.Dashboard.dll" ]
