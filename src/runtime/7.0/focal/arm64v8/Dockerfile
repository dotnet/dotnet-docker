ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22065.6/dotnet-runtime-7.0.0-alpha.1.22065.6-linux-arm64.tar.gz \
    && dotnet_sha512='d9917b2d53cdd74c0f24d0a01ac08fc583f90275c399a1996ce3c36057b2ce8e6770aac4f7884155eb2ab88e4359ccbc472fed95a473ecefe02bf853eb1f5482' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.22065.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
