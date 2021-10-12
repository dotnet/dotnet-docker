ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21480.5/dotnet-runtime-6.0.0-rc.2.21480.5-linux-arm64.tar.gz \
    && dotnet_sha512='b0f8128d994b0de0c72b9dd8083a7350218adfafedc643c59b497fa605420602004d9e21e8acb488d92bc498f9783a3240cfcafa77443eb6f08b66aadc4f5b65' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.2.21480.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
