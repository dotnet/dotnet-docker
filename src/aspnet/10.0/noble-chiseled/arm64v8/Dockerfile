ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25120.3 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='4f996f48fd215b63b84daf24a4ab08dd5b6877dab6bef0e6c35ec945152b6e5b22dcba9d50ac1198d5876313f0bcd0c800aa5a8cc83ec7647e77466f74dac22e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25120.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
