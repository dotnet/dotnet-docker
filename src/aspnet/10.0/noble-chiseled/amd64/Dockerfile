ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25126.5 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='bb1a6aa0ec9db62549a850a404767b88c7ee67c24d6e3cf1558b9a935ae807c9dc56ade5679dfe97431e696ee459b42c4ec95f5dd32a9aabcf0bb2ae32fb4dd9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-noble-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25126.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
