ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=13.6.0-preview.1.26459.12 \
    && curl --fail --show-error --location --output aspire_dashboard.zip https://ci.dot.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-x64.zip \
    && aspire_dashboard_sha512='cc1e24d394c7fc7e55ea6dbd5979aa06f625475afb50df0382aef517e4bad9ea8952aa775db6934ffa66658d560c832b2fcd05b787369edee415dd1efe57c08e' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:8.0.30-azurelinux3.0-distroless-extra-amd64

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
