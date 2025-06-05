ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.6 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='8a7024bd144254f400c0758efd5c39854eba5d7e3187fbde2dc857cedd9012ae93aceeb1683bf6bf390cefba40c50f95cc3295a1a8eb83133a8e01b91289dfc5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.6-bookworm-slim-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
