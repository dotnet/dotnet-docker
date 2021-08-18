ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-rc.1-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21417.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='7fd7ec3e0d836916a09370e150b522e7dc1987d776c595a55cc21b9d67e6903f2257a096e4f1721059bf8f4ce5ac41031501ef7fa5dfd5c6850c3337583a78d2' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
