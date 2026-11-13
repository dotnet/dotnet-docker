ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=17.1.0-preview.1.26562.9 \
    && curl --fail --show-error --location --output aspire_dashboard.zip https://ci.dot.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-x64.zip \
    && aspire_dashboard_sha512='d7d5a3737dedfb9290b64aa3d2da743e281127533d1f0f170ab804e562986053480b7395cfb020377bc872515b547476344c38115870d09485cc43bfbbcc9435' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:11.0.0-azurelinux3.0-distroless-extra-amd64

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
