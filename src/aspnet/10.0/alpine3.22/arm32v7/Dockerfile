ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25277.114 \
    && wget --output-document aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='789ec6dcb6805ed709f03facd19e2cf04faeb88408e2436bc7e7e76eb65f273402f4714cee60c6b08fd9b59138bff43b805965d1d5c5ba585beae71e5ffb8b4d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
