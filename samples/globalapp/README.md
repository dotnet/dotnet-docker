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

```bash
$ docker run --rm debian date
Mon Sep 16 16:17:01 UTC 2024
$ docker run --rm -e TZ="Etc/UTC" debian date
Mon Sep 16 16:17:31 UTC 2024
$ docker run --rm -e TZ=$"America/New_York" debian date
Mon Sep 16 12:17:51 EDT 2024
$ docker run --rm -e TZ=$"America/Los_Angeles" debian date
Mon Sep 16 09:18:08 PDT 2024
$ docker run --rm -e TZ=$(cat /etc/timezone) debian date
Mon Sep 16 09:19:26 PDT 2024
```

The first approach uses the default timezone, which is [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time). The other examples pass specific timezones, including UTC. The last pattern passes the timezone of the host.

A machine configured to UTC will produce the following:

```bash
# cat /etc/timezone
Etc/UTC
```

## Running the app

The app produces the following output, for the "America/Los_Angeles" timezone:

```bash
$ docker build --pull -t globalapp .
$ docker run --rm -it -e TZ="America/Los_Angeles" globalapp
Hello, World!

****Print baseline timezones**
Utc: (UTC) Coordinated Universal Time; 09/16/2024 16:22:03
Local: (UTC-08:00) Pacific Time (Los Angeles); 09/16/2024 09:22:03

****Print specific timezone**
Home timezone: America/Los_Angeles
DateTime at home: 09/16/2024 09:22:03

****Culture-specific dates**
Current: 09/16/2024
English (United States) -- en-US:
9/16/2024 4:22:03 PM
9/16/2024
4:22 PM
English (Canada) -- en-CA:
9/16/2024 4:22:03 p.m.
9/16/2024
4:22 p.m.
French (Canada) -- fr-CA:
2024-09-16 16 h 22 min 03 s
2024-09-16
16 h 22
Croatian (Croatia) -- hr-HR:
16. 09. 2024. 16:22:03
16. 09. 2024.
16:22
jp (Japan) -- jp-JP:
9/16/2024 16:22:03
9/16/2024
16:22
Korean (South Korea) -- ko-KR:
2024. 9. 16. 오후 4:22:03
2024. 9. 16.
오후 4:22
Portuguese (Brazil) -- pt-BR:
16/09/2024 16:22:03
16/09/2024
16:22
Chinese (China) -- zh-CN:
2024/9/16 16:22:03
2024/9/16
16:22

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
