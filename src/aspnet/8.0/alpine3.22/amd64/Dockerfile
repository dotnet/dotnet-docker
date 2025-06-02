ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.16-alpine3.22-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.16 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='2f8569e1335bd25a1c93be52364dd1386c60d70c5f905ca830093125de20bfd99be0808cf98f6aafd6a0bf0867962d7436a54ea7b8eb42a6fb31eeb5b08e39a2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.16-alpine3.22-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
