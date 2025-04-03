ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.15-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.15 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='4a72cb3e0a3994ac9ad8c18be64a6189d56221274cff31da74bf0b8aba02d68260f056a5734773e4c4a34851fe42e614e7720eb5197328f5e76076f850c576f3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.15-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=8.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
