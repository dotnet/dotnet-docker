ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.4.25258.110  \
    && curl -fSL --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='177a931c53532a4d5881775b0670405bd23ff12102c8f7c55f83a5a5db04c3d326e78eb6e86626a19f0a771067af3e23147c8bfd19bef56f21a44827bdd67a63' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.4-noble-chiseled-extra-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.4.25258.110 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.4.25258.110

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
