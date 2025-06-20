ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.5.25277.114 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='ec21eff51b0f8b9e7c9c00b6cc9e9cdd810997c9d2c1987042c48cdd6bf6e40c2552acbfcda405c56b3ce7de52b3a20e6b91f068c6e4bd101b896c302a32b5f2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-distroless-extra-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
