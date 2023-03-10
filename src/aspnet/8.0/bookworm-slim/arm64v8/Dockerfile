ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET composite runtime
RUN aspnetcore_version=8.0.0-preview.4.23221.9 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
    && dotnet_sha512='47a4c47e256bd0f15df8aa07f8765cf82f4e81e9b2e4214576800b81771877909e5b2ee149fc469e894d33f585d4ea0972afb29f6891a88dc1c65625ba85b4aa' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# ASP.NET composite runtime image
FROM $REPO:8.0.0-preview.4-bookworm-slim-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-preview.4.23221.1
# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.4.23221.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
