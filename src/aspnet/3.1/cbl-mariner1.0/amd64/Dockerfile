ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.27-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.27 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='cc200cfafc282ddde83ad3f5e2037e50f74fc15267f52f2123397adc14a246a2ab87e3ef2c8953d83f75022832ac9adb142dbc7010b969c1c913566d78025ce2' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
