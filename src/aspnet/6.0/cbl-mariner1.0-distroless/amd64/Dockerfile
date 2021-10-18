ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Acquire ASP.NET Core
RUN aspnet_version='6.0.0-rtm.21517.3' \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnet_version/aspnetcore-runtime-$aspnet_version-linux-x64.tar.gz \
    && aspnetcore_sha512='8d4319afa8251f81cb2a788305c2e29026733a024c7f57f760af856ee849953d7436b18cbd42f19eca018494fd4dad0d8a3496b70c0e52ab0048f10a4d5057c5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-cbl-mariner1.0-distroless-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21517.3 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
