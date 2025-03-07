ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.2.25156.10  \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
    && dotnet_sha512='a37b369c68b79a2d45737f791783d8aa4881850ecdf5d7b053d6dff66c14e67040a32988432a796ee99e5bba9430fc8a2b5dad7a93b78569d51624083829a895' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.2-noble-chiseled-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.2.25156.1 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.2.25156.10

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
