ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.0-preview.2-cbl-mariner2.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.2.24128.4 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='9d836edc539ace64ef8fa883bdfc881d89f4cf30d048640246dae9d54e46e79f2e82ebcdf366c1b69017d86d1bf1496acef5d56c3133297ea0bddb2df2eb4523' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.2-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.2.24128.4

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
