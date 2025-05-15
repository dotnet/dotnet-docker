ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.4.25258.107  \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
    && dotnet_sha512='27da0a48ca1d46ac77cc6b408ef7d8b420e62a72aec8b0cb84968fd4dc2a822d2151cfd3de76d868c1dc707e730ce188450b2e315285123bb8646589f247956e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.4-noble-chiseled-extra-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.4.25258.107 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.4.25258.107

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
