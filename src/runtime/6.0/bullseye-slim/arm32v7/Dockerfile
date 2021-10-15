ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21514.7/dotnet-runtime-6.0.0-rtm.21514.7-linux-arm.tar.gz \
    && dotnet_sha512='838bf9e7fcde1d289b41a2506be4797c4eaef414f7f5a9b03621fb21d653543f25f501ff90dbe2d4a398df4f61a58b7c380c68b4781ed3cb58d28e9da9ef8465' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rtm.21514.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
