ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.17 \
    && curl --fail --show-error --location --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='1308565bba86f891e5c14fe2295cc9e7b8e0c72830c8fab2d6a0d591de9667d8c457845a50f66f05d3218a4d2b1dd43b08ae3810d8672c5a5a3789f46ecb18d5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum --check - \
    && mkdir -p /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.17-bookworm-slim-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
