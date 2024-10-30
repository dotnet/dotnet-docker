ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rtm.24516.19 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-9.0.0-linux-arm.tar.gz \
    && aspnetcore_sha512='94fb8d4be44f1d7fd154f2963b7e9bb7c72d6f6a11bbcd0e0e9d870b9bc8081962a5cb6b14468ab5a0c5ecb4fa5e3e5f093c20d8d44b7dd4dfffba2f74203021' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rtm.24516.19

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
