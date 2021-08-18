ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21417.1/dotnet-runtime-6.0.0-rc.1.21417.1-linux-x64.tar.gz \
    && dotnet_sha512='42c39bec12bd8784deb8000bfe978f630c18b760ce7030e1a51840b9d40e98e37978fdb74c2ba15f7d47dd226058942d369589735798add2959809a5ede9f5a3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21417.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
