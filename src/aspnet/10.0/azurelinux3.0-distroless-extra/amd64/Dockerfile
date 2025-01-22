ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-alpha.2.25070.3 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='5348e5f44b2b245cdbeb509a410f992745a77f9b7f1e509fd0c731b886eaab6911527e5ca765d6d6083b22326d7545c75558a1f2d70a80db5cddaee6f63a484b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-alpha.1-azurelinux3.0-distroless-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25070.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
