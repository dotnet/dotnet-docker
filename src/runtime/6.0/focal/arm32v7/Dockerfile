ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=6.0.36 \
    && curl -fSL --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='f3f9199a97db81d3e3ed249e0e9b8f17afb785e8b9ebad00016576382d3d0f2029a31eaf760be2509fde796a42efeb727b156488a42f48cc08013f87479cacfd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.36-focal-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=6.0.36

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
