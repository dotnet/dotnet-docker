ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21426.17/dotnet-runtime-6.0.0-rc.1.21426.17-linux-arm.tar.gz \
    && dotnet_sha512='44b38f40f34b2affaa5a0fbc67c98a0dbbdaa4bdce9c37ffb244ea72ce087eaba7ef4f72db5985b2edcd3b120964fa3b54ca75940533b0696aa68f837d44fe4e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21426.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
