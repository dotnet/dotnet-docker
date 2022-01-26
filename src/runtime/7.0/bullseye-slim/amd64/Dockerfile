ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-preview.2.22074.8/dotnet-runtime-7.0.0-preview.2.22074.8-linux-x64.tar.gz \
    && dotnet_sha512='9e4b124d348b21d7a508d26fff9207ff062d1beb34c4990765bb84e36becc5a12df0f806017d49189c2a1af3299df41a49d47b45add1f0d23f081c9e1b79f147' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.2-bullseye-slim-amd64

ENV DOTNET_VERSION=7.0.0-preview.2.22074.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
