ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.3-alpine3.20-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.3 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='bd807c994b02aa0c97e001492c16146894f6aa1fdef12d0979ffb0c84d809cdf955fd351016bb59d5098ec945fc3cce7958a6fedcd0e301ef28159700e380278' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.3-alpine3.20-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
