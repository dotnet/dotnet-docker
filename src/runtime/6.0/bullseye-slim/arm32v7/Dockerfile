ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21377.19/dotnet-runtime-6.0.0-preview.7.21377.19-linux-arm.tar.gz \
    && dotnet_sha512='9aec69078629bdf2c84c91edc647ab05d50845d90cd698f22d39f6f54ba7d9bf3050ac87797fa7fe4c098a0bb63f456ecaa050b8504359d365fac22d40e74fea' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.7.21377.19

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
