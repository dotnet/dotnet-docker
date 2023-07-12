# Enabling (or disabling) globalization functionality

.NET includes various [globalization](https://learn.microsoft.com/dotnet/core/extensions/globalization-and-localization) capabilities, including support for processing natural language text, calendars, currency, and time zones. The .NET implementations for these capabilities comes primarily from operating system libraries, such as [International Components for Unicode (ICU)](https://icu.unicode.org/) and [tzdata](https://wikipedia.org/wiki/Tz_database). In some cases, these libraries and their accompanying databases are always available and in other cases it is desired that they be absent because they may be considered prohibitively large.

The .NET team has various policies for making these libraries available in containers and for configuring the .NET product to use them (or not).

The [globalapp](globalapp/README.md) samples demonstrates using globalization capabilities in containers.

## ICU

In many scenarios, globalization support with ICU is required, for example, to correctly sort a list of strings (particularly characters outside the [ASCII](https://en.wikipedia.org/wiki/ASCII) range). Other applications may not be oriented around natural language or other global concepts, and would prefer to optimize for container size. .NET container images have been curated to offer options for both of these scenarios.

.NET container images that include ICU:

- Alpine `sdk` images
- Debian images
- Ubuntu images

.NET container images that do not include ICU:

- Alpine `aspnet`, `monitor`, `runtime`, `runtime-deps` images
- Ubuntu chiseled images

Images that do not include ICU enable [Globalization Invariant Mode](https://aka.ms/dotnet/globalization/invariant), which provides more basic globalization behaviors in absence of using ICU.

Some users want to add ICU to one of the image types that doesn't include it. It is counter-productive to remove ICU from an image that already includes it; it doesn't actually reduce the size of the image since it's stored in an earlier layer which cannot be changed.

### Use of `Microsoft.Data.SqlClient` requires ICU to be installed

When using `Microsoft.Data.SqlClient` or Entity Framework Core without ICU installed, the following exception may be thrown when attempting to connect to a database:

```
System.Globalization.CultureNotFoundException: Only the invariant culture is supported in globalization-invariant mode. See https://aka.ms/GlobalizationInvariantMode for more information. (Parameter 'name')
en-us is an invalid culture identifier.
```

This is by design. `Microsoft.Data.SqlClient` requires ICU to be installed. See https://github.com/dotnet/SqlClient/issues/220 for more information.

### Alpine images

ICU can be added to a .NET Alpine image by adding the following instructions to the final stage within a `Dockerfile`, as demonstrated in [Dockerfile.alpine-icu](aspnetapp/Dockerfile.alpine-icu). This Dockerfile fragment adds and configures ICU and disables Globalization invariant mode.

```Dockerfile
ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

RUN apk add --no-cache \
    icu-data-full \
    icu-libs
```

### Ubuntu chiseled images

ICU can be added to an Ubuntu chiseled image, as demonstrated by https://github.com/ubuntu-rocks/dotnet/issues/21.

## Tzdata

Tzdata provides data for applications that rely on timezone information. Applications that solely use `DateTime.UtcNow` for recording time don't need this library.

The following code uses `tzdata`:

```csharp
DateTime nowUtc = DateTime.UtcNow;
DateTime now = DateTime.Now;
var homeZone = TimeZoneInfo.Local;
string home = "America/Los_Angeles";
var tz = TimeZoneInfo.FindSystemTimeZoneById(home);
```

With `tzdata` installed and the timezone configured (via the `TZ` environment variable or writing a value to  `/etc/timezone`), `DateTime.UtcNow` and `DateTime.Now` will return the correct values (which will be same if the timezone is `Etc/UTC`), `TimeZoneInfo.Local` will return the value set in `/etc/timezone`, and the call to `TimeZoneInfo.FindSystemTimeZoneById` will succeed.

Without `tzdata` installed, `DateTime.UtcNow` and `DateTime.Now` will return the same value, `TimeZoneInfo.Local` will return a value for UTC, and the call to `TimeZoneInfo.FindSystemTimeZoneById` will fail resulting in an exception.

.NET container images do not install `tzdata`, however, Debian images contain it (at the time of writing). If you rely on this library, you need to install it by adding the following instructions to the final stage within a `Dockerfile`, depending on the distro you use.

### Alpine

`tzdata` can be added to a .NET Alpine image with the following `Dockerfile` fragment.

```bash
RUN apk add --no-cache tzdata
```

### Ubuntu

`tzdata` can be added to a .NET Ubuntu image with the following `Dockerfile` fragment.

```bash
apt update && DEBIAN_FRONTEND=noninteractive && apt install -y tzdata && rm -rf /var/lib/apt/lists/*
```

### Launching a container with timezone information

The best practice is to pass timezone information into a container via environment variable.

```bash
docker run --rm -it -e TZ="Etc/UTC" app
```

Alternatively, the host timezone can be used:

```bash
$ cat /etc/timezone
America/Los_Angeles
$ docker run --rm -it -e TZ=$(cat /etc/timezone) app
```

This approach enables a container image to be launched with a specific timezone (at launch), as opposed to setting the timezone in the image as part of `docker build`.
