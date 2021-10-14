ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21513.22/dotnet-runtime-6.0.0-rtm.21513.22-linux-arm.tar.gz \
    && dotnet_sha512='637b8881988803eb055bdc153237c7eb74429f512397724f5bc1896ed904076acb9218d35616ceb3e83b52e10e0c5e35d7c18cfd62699ee4d73df7d6ad22bdb4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rtm.21513.22

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
