ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install --assumeyes \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.6.25274.104 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='b96f77e58dd6e185b436197366ed65dd8be0faddacc17b85cad4ffea4bea7304b615c6ffb5a9580c6fe7da4d1047bfb2971825b3028ac1724816072c318c8a1d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln --symbolic /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:10.0.0-preview.6-azurelinux3.0-distroless-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
