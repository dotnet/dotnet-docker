ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-rc.2.25474.113 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz.sha512 \
    && echo "$(cat dotnet-runtime-$dotnet_version-linux-x64.tar.gz.sha512)  dotnet-runtime-$dotnet_version-linux-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-runtime-$dotnet_version-linux-x64.tar.gz --directory /usr/share/dotnet \
    && rm \
        dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
        dotnet-runtime-$dotnet_version-linux-x64.tar.gz.sha512

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:10.0.0-rc.2-noble-chiseled-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-rc.2.25474.113

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
