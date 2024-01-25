ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
# FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer
FROM $REPO:8.0.1-cbl-mariner2.0-amd64 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        # tar \
        unzip \
    && tdnf clean all

WORKDIR /tmp

COPY Aspire.Dashboard.Sdk.linux-x64.8.0.0-dev.nupkg .

RUN mkdir -p /app \
    && unzip Aspire.Dashboard.Sdk.linux-x64.8.0.0-dev.nupkg -d /app \
    && rm Aspire.Dashboard.Sdk.linux-x64.8.0.0-dev.nupkg


FROM $REPO:8.0.1-cbl-mariner2.0-distroless-amd64

WORKDIR /app

COPY --from=installer /app/tools .

ENV \
    # ASPNETCORE_HTTP_PORTS=18888 \
    # Server GC mode
    DOTNET_gcServer=1

ENTRYPOINT [ "dotnet", "/app/Aspire.Dashboard.dll" ]
