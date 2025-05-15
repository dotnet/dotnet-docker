ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.4.25258.107  \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='06c5b9f5e68cb9fca4086818cbe4f02fe4dec5b4cad7ec9da7b9bd5f68f2c5a32aecc027f08289bf21560f0298494ca425ed30f1cffc0a3e4e8350536269a9c6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.4-noble-chiseled-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.4.25258.107 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.4.25258.107

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
