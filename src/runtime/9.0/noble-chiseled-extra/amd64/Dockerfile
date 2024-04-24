ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-preview.4.24223.11 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='bf350edaecea17e445c1782a8164c35d9add3a520e719cfb0d8ba7250bc7ffe85b9be8a4a75ab75f9b90d00b7731014f0ef8048f6db35e30577e002ae9f03e53' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:9.0.0-preview.4-noble-chiseled-extra-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.4.24223.11

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
