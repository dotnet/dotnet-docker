ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.5.25280.105 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='462c83ce2213260d82ee80cd7ffa183b85f4ab8afaf485fa07ef5b80c1292975d15066063afa70908b4c27a17a6c03625611beb42940938d46e80f832b2138c7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.5-trixie-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.5.25280.105

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
