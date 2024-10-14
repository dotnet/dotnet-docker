ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=9.0.0-rc.1.24511.1 \
    && curl -fSL --output aspire_dashboard.zip https://dotnetbuilds.azureedge.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-arm64.zip \
    && aspire_dashboard_sha512='c6d0a9da8e459dd64f24758b60cc8b587cbf1c1300188f924d28aea3ace483858b1bb1287782121067af00b9d5533a529a070e8b376f85f7775ffd03c4b193fd' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir -p /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:9.0.0-rc.2-cbl-mariner2.0-distroless-extra-arm64v8

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
