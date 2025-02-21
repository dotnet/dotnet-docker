ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.13-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.13 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='a8c284406cdbbb8ef90aede0a9507deaf3cefc7966ca8de04576bbb3bae260b81b16b2441d4d4385792ecc3417ea1ab3aeceab5497f8665e70dfa55ef7e5af2b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.13-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=8.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
