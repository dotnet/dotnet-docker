ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.0-rc.1-azurelinux3.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.1.24452.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='f8fd285d67bb044d631596869d6301e10a2a243c81c9a05096a66aff4fb3431529812c7482e6cf0e065e8e065fc50b16b50d7f2a495ab30077a68bd45b3ba376' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.1-azurelinux3.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.1.24452.1

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
