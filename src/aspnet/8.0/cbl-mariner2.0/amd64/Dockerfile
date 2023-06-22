ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.0-preview.6-cbl-mariner2.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.6.23320.14 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='1ee205a511f323d9864d73f51dea8b99297fe2ee93844a098884ce73d2323dc8614997c7ba2e67250da222fb2d0d27ae259f8658d8d6e223d011c31b7eb4114d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23320.14

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
