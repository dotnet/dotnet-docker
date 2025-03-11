ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25160.5 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='fcf536edb6e35412e13ad3568e8d71c2b30e40a21abed2e909aa88e190ecbc6ec27ba925f7de946e4b1d14a161ba88b58e6c8145046e319218afa5023d033892' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-trixie-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25160.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
