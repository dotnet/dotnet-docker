ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.21562.1/dotnet-runtime-7.0.0-alpha.1.21562.1-linux-arm.tar.gz \
    && dotnet_sha512='4d7622f0bf80c30a40fa647b5cf93ae1c089a156177714bac8c2626ce8ce1683bfbba2112af2cd2825d36ed2fe4bbe050e45493ec47c5fb6b6a2a06227cea015' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.21562.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
