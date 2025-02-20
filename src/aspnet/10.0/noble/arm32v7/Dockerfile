ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25107.4 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='80c2808be8c58fec05aec3d0db79eb31d1517244541a31de5b1e82a486ebb4389f8fae0823df6e8070a132e21b531afefe516bc42f3122fa06a91e48c178efa0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-noble-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25107.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
