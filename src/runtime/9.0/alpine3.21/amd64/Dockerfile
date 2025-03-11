ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.3-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.3 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='8533a061f4fad61135269ae7987d9bdc8000cbc4999e00392f0e9d234d40e20a4f4e753dd5724373772349a896f2a84e3ba86e876d81a9bf068888e421afb13f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.3-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
