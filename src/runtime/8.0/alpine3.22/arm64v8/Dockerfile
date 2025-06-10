ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.17-alpine3.22-arm64v8 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.17 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='807e8b4576e71962406349f32506a5b4abd7291d4bf7c951e5eb80fcaccf77cc3f1b57fe4e7110d0b7169554e0e8905c4988bb7417f0309bdecbcadcaaecbd66' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.17-alpine3.22-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
