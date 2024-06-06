ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.5.24305.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='9b3006335770b7a26535abd2f4ad3c2a6c1c6572beb20c555da594418ce236e11cdca94e28212f1bcb9eff27322007721e15ccb5d5ac384c5d70bcf7b78c93e0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.5-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.5.24305.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
