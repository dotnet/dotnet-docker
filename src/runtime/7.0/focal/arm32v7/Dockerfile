ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22065.6/dotnet-runtime-7.0.0-alpha.1.22065.6-linux-arm.tar.gz \
    && dotnet_sha512='4fcd36518a334b9d1cdddec3534e697aca0ef21453d4d636aa98058fd774d470f1ed367dab1fe79ebdecdb8dfed7a5edab2346f7b60ddcfdf507c544510c70a5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.22065.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
