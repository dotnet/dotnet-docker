ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        unzip \
    && tdnf clean all

# Retrieve Aspire Dashboard
RUN dotnet_aspire_version=13.1.0-preview.1.25574.3 \
    && curl --fail --show-error --location --output aspire_dashboard.zip https://ci.dot.net/public/aspire/$dotnet_aspire_version/aspire-dashboard-linux-x64.zip \
    && aspire_dashboard_sha512='662eddbab92c76dc05b007a0879ab99490385839fb8b780b16146c1c4b9d9fed29fbbf4a13b6c761bbc527fd5508ec8c3e39cf6f4a830d573117c278102231ed' \
    && echo "$aspire_dashboard_sha512  aspire_dashboard.zip" | sha512sum -c - \
    && mkdir --parents /app \
    && unzip aspire_dashboard.zip -d /app \
    && rm aspire_dashboard.zip


# Aspire Dashboard image
FROM $REPO:8.0.22-azurelinux3.0-distroless-extra-amd64

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
