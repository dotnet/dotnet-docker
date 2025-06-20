ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.6.25274.104 \
    && curl --fail --show-error --location --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='6ef2d6e21c75a79a6fbe172b3633cc33a4e9df7d4cb845c52038ec50499c92ef15773c8840b30d01ac99ca03a972172d70c36829f74622449be6251b3914a452' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-noble-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
