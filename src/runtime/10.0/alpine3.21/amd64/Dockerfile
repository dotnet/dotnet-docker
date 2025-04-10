ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.3.25171.5 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='26914a60143fa394d1f238d4da335ae5869696899b31da9b1f537df96e2f4b1862cdab11ea2e550dae9a19817ee3679aaa0e4db9656cebe5143b8513090dfee7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.3.25171.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
