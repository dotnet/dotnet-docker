ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.6.25274.104 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='88a473f51a142dd7a5512ab3aba8889b6e0f6602aad82d16666bb45381af71d8478b190a16d9b461f7ff0ed1f0318795731b9b702a0c71546d2b8ce0e5f63597' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.6-noble-chiseled-extra-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.6.25274.104 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
