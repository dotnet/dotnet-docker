ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0 \
    && dotnet_build_version=$dotnet_version-rtm.25515.111 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/Runtime/$dotnet_build_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/Runtime/$dotnet_build_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz.sha512 \
    && echo "$(cat dotnet-runtime-$dotnet_version-linux-arm.tar.gz.sha512)  dotnet-runtime-$dotnet_version-linux-arm.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-arm.tar.gz --directory /usr/share/dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
        dotnet-runtime-$dotnet_version-linux-arm.tar.gz.sha512

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:10.0.0-noble-chiseled-extra-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-rtm.25515.111

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
