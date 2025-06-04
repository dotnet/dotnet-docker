ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25280.105 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='6c6e421abcb91c33c661595f279ed243aca119deff4fcc02b2254649739772b07dfda2487293abc4018085d1187f90471603c2b4db5615f207d7997de9419b0f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25280.105

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
