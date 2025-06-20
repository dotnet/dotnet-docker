ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.5.25277.114 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
    && dotnet_sha512='2160222cfbd063f1ef109d94c629a2d2236263922927accb30bf38ff1f853f69651fa33df5c96aa2ac037c2d6aef7a107347f7e40ea3681eefbccec7d06d1d1c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.5-noble-chiseled-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25277.114 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
