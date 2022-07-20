ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.7-servicing.22322.5 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='0c7317d2170f2632afd7c7c3e5bd84075071802e901de1ed5e54178f8a56266fe0770ebd84502aff9384b06908d4d5bee9d464d215fb20d841de177174f55f93' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.7-jammy-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.7-servicing.22322.5

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
