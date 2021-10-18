ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21517.2/dotnet-runtime-6.0.0-rtm.21517.2-linux-x64.tar.gz \
    && dotnet_sha512='0e2596eca335cac1c7dc6015e70dd8dd063a7a534e56fbeb4fc44838b845feb311b088e9535763620c04f31665911f9d3acdec1295cefae76baa0e68dc26d0a9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21517.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
