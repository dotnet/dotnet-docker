ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22062.8/dotnet-runtime-7.0.0-alpha.1.22062.8-linux-arm64.tar.gz \
    && dotnet_sha512='3ec7c10ba921289bdd20a40b36956ec52caf1dd6a282404cb6c67dbe0bcf71ffefe5e228877e698ffdac7cd7af95fc262366ed4526a135234591c68e64eda204' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.22062.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
