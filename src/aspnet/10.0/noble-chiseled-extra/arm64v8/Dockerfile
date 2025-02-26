ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25125.7 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='a3efb29145f597129b45fee321ba01c772e7f19c532029a1cee9f84553e5725960e0db65cb35e176e659d2cc97b565eb1ebdf00ac55c734df24f330b0afb963b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25125.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
