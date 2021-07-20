ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21369.8/dotnet-runtime-6.0.0-preview.7.21369.8-linux-x64.tar.gz \
    && dotnet_sha512='7c2f54435f952431108ddb18e2aa0b8c55a3913c62e23b8105c795f25da40e97364f1d8f7abe98b95f32e4592cf693148cd31eb0cec4ae852c9c0231e179a3b0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-amd64

ENV DOTNET_VERSION=6.0.0-preview.7.21369.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
