ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.5.25269.109 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='7c698ffeab5ca0f385898aad5d0c960156b157a778e444d5be9e78911880cacc114a1d1445c96047fa5c717132d51b152e3cbc47d8179a4e5ce3d19fe1de31e0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.5-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.5.25269.109

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
