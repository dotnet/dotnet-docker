ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21377.19/dotnet-runtime-6.0.0-preview.7.21377.19-linux-x64.tar.gz \
    && dotnet_sha512='959ef85c0cb5c9aa8dab90f5e09d9f9216d546a32b5338f389787523b9c4b48b911ac42a4e6f8c9e1b656bc8f0173ff93beed12439e3f87a52b84d810969969a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-preview.7.21377.19

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
