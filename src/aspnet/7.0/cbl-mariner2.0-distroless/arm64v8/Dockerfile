ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner2preview.azurecr.io/base/core:2.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-preview.3.22158.12 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='e52269cba9ec45f18543d1c02a26183ca3dbcb988a203f175ba1227f70bcdb18f23a7b08fd837ce67abc7f8b76cfea96d100d924328681e6055ae0696c18414c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.3-cbl-mariner2.0-distroless-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.3.22158.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
