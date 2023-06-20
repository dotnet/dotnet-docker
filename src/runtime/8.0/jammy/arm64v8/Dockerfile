ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-preview.6.23316.3 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='f9202761e31a9efedf612d8434d007e6e02d525213ff11abbe1f6b4cc03225ed4fe09e1f5af384ec9f95494aea06464a52ee3026363451f287b02e40df4dc551' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-preview.6-jammy-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-preview.6.23316.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
