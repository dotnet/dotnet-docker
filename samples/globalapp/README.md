# Globalization Test App

This [app](Program.cs) is intended for testing .NET APIs that use globalization APIs and concepts. The behavior of these APIs varies depending on how a container (or other environment) is configured. The [Dockerfile](Dockerfile) is intended to aid testing and isn't itself an example of a production Dockerfile.

See [.NET Docker Samples](../README.md) for more samples.

## Configuration

The behavior of these APIs is affected by:

- Presence of `icu` and `tzdata` packages.
- The value of [globalization invariant mode](https://aka.ms/GlobalizationInvariantMode).
- The value (or absence) of the `TZ` environment variable.
- The value (or absence) of `/etc/timezone`

The recommended way to configure [tzdata](https://en.wikipedia.org/wiki/Tz_database) and [timezones](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones) is to set the container timezone by using the `TZ` environment variables, as is demonstrated below.

```console
$ docker run -it --rm -e TZ=$(cat /etc/timezone) mcr.microsoft.com/dotnet/runtime-deps:10.0

root@4770a50f643e# date
Wed Sep 10 17:27:24 UTC 2025

root@4770a50f643e# TZ="Etc/UTC" date
Wed Sep 10 17:27:42 UTC 2025

root@4770a50f643e# TZ="America/New_York" date
Wed Sep 10 13:28:26 EDT 2025

root@4770a50f643e# TZ="America/Los_Angeles" date
Wed Sep 10 10:29:07 PDT 2025

root@4771a50f643e# exit

$ docker run --rm -e TZ=$(cat /etc/timezone) mcr.microsoft.com/dotnet/runtime-deps:10.0 date
Wed Sep 10 10:29:07 PDT 2025
```

The first approach uses the default timezone, which is [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time). The other examples pass specific timezones, including UTC. The last pattern passes the timezone of the host.

A machine configured to UTC will produce the following:

```console
$ cat /etc/timezone
Etc/UTC
```

## Running the app

The app produces the following output, for the "America/Los_Angeles" timezone:

```console
$ docker build --pull -t globalapp .
$ docker run --rm -it -e TZ="America/Los_Angeles" globalapp
Hello, World!

****Print baseline timezones**
Utc: (UTC) Coordinated Universal Time; 09/10/2025 17:35:50
Local: (UTC-08:00) Pacific Time (Los Angeles); 09/10/2025 10:35:50

****Print specific timezone**
Home timezone: America/Los_Angeles
DateTime at home: 09/10/2025 10:35:50

****Culture-specific dates**
Current: 09/10/2025
English (United States) -- en-US:
9/10/2025 5:35:50 PM
9/10/2025
5:35 PM
English (Canada) -- en-CA:
2025-09-10 5:35:50 p.m.
2025-09-10
5:35 p.m.
French (Canada) -- fr-CA:
2025-09-10 17 h 35 min 50 s
2025-09-10
17 h 35
Croatian (Croatia) -- hr-HR:
10. 09. 2025. 17:35:50
10. 09. 2025.
17:35
jp (Japan) -- jp-JP:
9/10/2025 17:35:50
9/10/2025
17:35
Korean (South Korea) -- ko-KR:
2025. 9. 10. 오후 5:35:50
2025. 9. 10.
오후 5:35
Portuguese (Brazil) -- pt-BR:
10/09/2025 17:35:50
10/09/2025
17:35
Chinese (China) -- zh-CN:
2025/9/10 17:35:50
2025/9/10
17:35

****Culture-specific currency:**
Current: ¤1,337.00
en-US: $1,337.00
en-CA: $1,337.00
fr-CA: 1 337,00 $
hr-HR: 1.337,00 €
jp-JP: ¥ 1337
ko-KR: ₩1,337
pt-BR: R$ 1.337,00
zh-CN: ¥1,337.00

****Japanese calendar**
08/18/2019
01/08/18
平成元年8月18日
平成元年8月18日

****String comparison**
Comparison results: `0` mean equal, `-1` is less than and `1` is greater
Test: compare i to (Turkish) İ; first test should be equal and second not
0
-1
Test: compare Å Å; should be equal
0
```
