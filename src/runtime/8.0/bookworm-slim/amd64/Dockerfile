ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bookworm-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.7 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='88e9ac34ad5ac76eec5499f2eb8d1aa35076518c842854ec1053953d34969c7bf1c5b2dbce245dbace3a18c3b8a4c79d2ef2d2ff105ce9d17cbbdbe813d8b16f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.7-bookworm-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
