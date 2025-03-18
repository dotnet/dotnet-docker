ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25164.1 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='de14dab978133b9ec979d371c5fbd01e78b106cc330c168d6216ec8a97b0ccfeeb0d4f44386187499760db603483dc0016279dc4cb3f94bfb042cc63d25296fa' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-trixie-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25164.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
