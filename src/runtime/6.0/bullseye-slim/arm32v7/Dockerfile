ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21517.2/dotnet-runtime-6.0.0-rtm.21517.2-linux-arm.tar.gz \
    && dotnet_sha512='b7861f93994372dc04da09910460509ee4505e642b93e26cef94e956fa7345a5cc3c4d5166efc4187d5b617da66aa2bc01c2e621d69fd3a39faaf5eee9c31bed' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rtm.21517.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
