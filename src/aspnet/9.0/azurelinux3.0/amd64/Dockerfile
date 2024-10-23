ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.0-azurelinux3.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rtm.24516.19 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='0c53d3dcddeabded9e9fabe273b5f5fe91babc81afe2b6e60919c8ee60665bfb453caff5a8424aabaa9ee55d93cd3d39d5bfacbaef6f04b4a647f9d3206918ca' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-azurelinux3.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rtm.24516.19

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
