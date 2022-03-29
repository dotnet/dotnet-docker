ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner2preview.azurecr.io/base/core:2.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-preview.3.22175.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='d899c91c915dfbb64db4329b346359a2072b060c067fb7110beb8659ab8d412ee80bc01b57f81eb23f933d53cb0bc7ce05566b34386569949cb3cb1b6d8c9302' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.3-cbl-mariner2.0-distroless-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.3.22175.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
