ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25258.110 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='64c4359a8d35ac15312ad844587dee1d227f207d3c9ba16edf50c6706f93a29d0b8d3388bd17547491652f1423a67bfbf42c4c7ae880afbe420615e529babf75' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.4-trixie-slim-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25258.110

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
