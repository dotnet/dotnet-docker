ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.5.25277.114 \
    && curl --fail --show-error --location --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='e0023293584c9df783f1ccb4a8368fe9dab128bc42b469caf883e52a6afdf0c216f001be0a9e383c537dfd5f31b8106944936784e5d74193d77e50eb08d453a7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.5-noble-chiseled-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25277.114 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
