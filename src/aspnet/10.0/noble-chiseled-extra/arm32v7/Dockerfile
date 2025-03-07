ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25156.10 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='4eda55f7cc53c348ff73bbac36db701a50d1cbbc5e5325d12ee184cd93f23dc9bd2f71649e655a79611086c819c276658fd161d3dc55bb9a417f7d2b955cb84f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25156.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
