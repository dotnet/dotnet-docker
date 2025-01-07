ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.12 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='9323f6584bf98500fe023009dea5b90e49bbb34cdcea0868e8d18c2fe260b087315438ca2df783f259003c1a0ee31f2d735c8cea85c2c4fb04f6dafe05384531' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.12-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
