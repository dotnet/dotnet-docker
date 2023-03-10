ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET composite runtime
RUN aspnetcore_version=8.0.0-preview.4.23221.9 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
    && dotnet_sha512='c1dc554665af00a931b882947d395eec4b92c95e4966a88e3993237e32d6f1147f34d2da560a93e7d5792239697f03bb7d3e80ef5fd10ff5cc6d5d973b0cd7dc' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# ASP.NET composite runtime image
FROM $REPO:8.0.0-preview.4-jammy-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-preview.4.23221.1
# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.4.23221.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
