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
Wed Feb 22 03:08:10 UTC 2023
$ docker run --rm -e TZ="Etc/UTC" debian date
Wed Feb 22 03:08:13 UTC 2023
$ docker run --rm -e TZ=$"America/New_York" debian date
Tue Feb 21 22:08:16 EST 2023
$ docker run --rm -e TZ=$"America/Los_Angeles" debian date
Tue Feb 21 19:08:39 PST 2023
$ docker run --rm -e TZ=$(cat /etc/timezone) debian date
Tue Feb 21 19:08:44 PST 2023
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
$ docker build --pull -t app .
$ docker run --rm -it -e TZ="America/Los_Angeles" app
Hello, World!

****Print baseline timezones**
Utc: (UTC) Coordinated Universal Time; 2/14/2023 12:09:26AM
Local: (UTC-08:00) Pacific Time (Los Angeles); 2/13/2023 4:09:26PM

****Print specific timezone**
Home timezone: America/Los_Angeles
DateTime at home: 2/13/2023 4:09:26PM

****Culture-specific dates**
Current: 2/14/2023
English (United States) -- en-US:
2/14/2023 12:09:26AM
2/14/2023
12:09AM
English (Canada) -- en-CA:
2/14/2023 12:09:26a.m.
2/14/2023
12:09a.m.
French (Canada) -- fr-CA:
2023-02-14 00 h 09 min 26 s
2023-02-14
00 h 09
Croatian (Croatia) -- hr-HR:
14. 02. 2023. 00:09:26
14. 02. 2023.
00:09
jp (Japan) -- jp-JP:
2/14/2023 00:09:26
2/14/2023
00:09
Korean (South Korea) -- ko-KR:
2023. 2. 14. 오전 12:09:26
2023. 2. 14.
오전 12:09
Portuguese (Brazil) -- pt-BR:
14/02/2023 00:09:26
14/02/2023
00:09
Chinese (China) -- zh-CN:
2023/2/14 00:09:26
2023/2/14
00:09

****Culture-specific currency:**
Current: $1,337.00
en-US: $1,337.00
en-CA: $1,337.00
fr-CA: 1 337,00 $
hr-HR: 1.337,00 €
jp-JP: ¥1,337
ko-KR: ₩1,337
pt-BR: R$ 1.337,00
zh-CN: ¥1,337.00

****Japanese calendar**
8/18/2019
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
