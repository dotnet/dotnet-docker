ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:7.0.12-cbl-mariner2.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.12 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='b302afe999bbb33a846222d52b5aa90c2538bd4ff6eabd431e3f50bebf7f501c3793ca2d4cea1c8f0fedef87fa05d045af63d947653e14c59fa33e23d297012d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.12-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.12

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
