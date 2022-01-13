ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22061.11/dotnet-runtime-7.0.0-alpha.1.22061.11-linux-arm64.tar.gz \
    && dotnet_sha512='f7a42a2bd24cd548521bddddf597a6c19b1a0660f516835fcf6b6859590e3a6d0996aa70bbe2ab02deb253dc85e26b944c8c99507467f84a425237981fbba439' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.22061.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
