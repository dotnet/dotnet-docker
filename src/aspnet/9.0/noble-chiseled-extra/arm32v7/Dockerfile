ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.1 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='fa75d8d5ae99ade0d1ab90018839fe3f5ddc4e7b7461715caf2b0bf7a88c8e86e1d4f10ab69703d2318b289c0700846e2155746d7bb1ace3d2d12e175ab18be1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.1-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
