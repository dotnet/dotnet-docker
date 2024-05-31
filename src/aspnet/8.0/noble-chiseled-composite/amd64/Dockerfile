ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=8.0.6  \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
    && dotnet_sha512='bffc08fb0ec63251d06cf69c95844d19924b139f0417d1a79f729c856fda89610e03b3ed49844f7dd15401c447137f6daf5b26550269a2c0eb0bff253ab234ec' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:8.0.6-noble-chiseled-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.6 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.6

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
