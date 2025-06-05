ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.17 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='4f1a441e400b60f814a161d2718c2599b4d492fffe5df5a5d8a494cec553ad3574c0988e9dc49abf8c928b9e9783a86f5506cbcbdf12d24e562090969aced3c5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.17-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
