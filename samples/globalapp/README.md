# Globalization Test App

This [app](Program.cs) is intended for testing .NET APIs that use globalization APIs and concepts. The behavior of these APIs varies depending on how a container (or other environment) is configured. The [Dockerfile](Dockerfile) is intended to aid testing and isn't itself an example of a production Dockerfile.

The behavior of these APIs is affected by:

- Presence of `icu` and `tzdata` packages.
- The value of [globalization invariant mode](https://aka.ms/GlobalizationInvariantMode).
- The value (or absence) of `/etc/timezone`

Assuming a correctly configured environment, the app produces the following output:

```bash
$ docker build --pull -t app .
$ docker run --rm app
Hello, World!

****Print baseline timezones**
Utc: (UTC) Coordinated Universal Time; 2/13/2023 12:19:40 AM
Local: (UTC-08:00) Pacific Time (Los Angeles); 2/12/2023 4:19:40 PM

****Print specific timezone**
Home timezone: America/Los_Angeles
DateTime at home: 2/12/2023 4:19:40 PM

****Culture-specific dates**
Current: 2/13/2023
English (United States) -- en-US:
2/13/2023 12:19:40 AM
2/13/2023
12:19 AM
English (Canada) -- en-CA:
2023-02-13 12:19:40 a.m.
2023-02-13
12:19 a.m.
French (Canada) -- fr-CA:
2023-02-13 00 h 19 min 40 s
2023-02-13
00 h 19
Croatian (Croatia) -- hr-HR:
13. 02. 2023. 00:19:40
13. 02. 2023.
00:19
jp (Japan) -- jp-JP:
2/13/2023 00:19:40
2/13/2023
00:19
Korean (South Korea) -- ko-KR:
2023. 2. 13. 오전 12:19:40
2023. 2. 13.
오전 12:19
Portuguese (Brazil) -- pt-BR:
13/02/2023 00:19:40
13/02/2023
00:19
Chinese (China) -- zh-CN:
2023/2/13 00:19:40
2023/2/13
00:19

****Culture-specific currency:**
Current: $1,337.00
en-US: $1,337.00
en-CA: $1,337.00
fr-CA: 1 337,00 $
hr-HR: 1.337,00 kn
jp-JP: ¥1,337
ko-KR: ₩1,337
pt-BR: R$ 1.337,00
zh-CN: ¥1,337.00

****Japanese calendar**
8/18/2019
01/08/18
平成元年8月18日
平成元年8月18日
```
