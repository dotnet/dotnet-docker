ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:trixie-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25258.107 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='89050750bbfa5782cfec7630c10a5b08cbda978de03a95ac4d8441b6529fd73abcbf372fdc69027ee5d249fcdd92d6dbddcf30e91e6bd378fbab2567806e7244' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.4-trixie-slim-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25258.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
