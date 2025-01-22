ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-alpha.2.25070.3 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='33b48387b71f0e7fac26956893e4bb0c860709527922c3db226e17fc1105c76b0516b8e0c75e7f8795a8c5e64ce8facfa739868c32d67e10221585b6817f8c59' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-alpha.1-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25070.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
