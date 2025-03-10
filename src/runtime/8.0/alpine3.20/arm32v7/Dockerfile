ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.14-alpine3.20-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.14 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='4f071b8e53c8046037bdc945c8dd61955df074820e8e7bc3922a97fe1bcc9574635c9a0aab643d5d4d5b8928de12d14435bd6eb097c189bd850d4f704dbbdadc' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.14-alpine3.20-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=8.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
