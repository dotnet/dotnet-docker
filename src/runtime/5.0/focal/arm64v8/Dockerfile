ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.11/dotnet-runtime-5.0.11-linux-arm64.tar.gz \
    && dotnet_sha512='b296a79f655ad22db405f24c1549b6cd610c57dae917c89b91ddcb4b053a7613952261cca7d7c79e6fda72026fd4bbbcf74751cb6529a55504a67223cabc0451' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.11-focal-arm64v8

ENV DOTNET_VERSION=5.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
