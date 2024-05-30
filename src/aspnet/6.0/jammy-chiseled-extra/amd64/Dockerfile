ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.31 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='ebb20a3461bf9d1e3a5c91761452f5ef2e60873826ad3158493768a18d207319bccc40d6c1a64fd61dd8c22bad51c26689394b0e6a40c4bfe4cca00ce4c00db1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.31-jammy-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.31

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
