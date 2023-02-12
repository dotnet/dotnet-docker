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
Utc: (UTC) Coordinated Universal Time; 02/12/2023 22:27:02
Local: (UTC-08:00) Pacific Time (Los Angeles); 02/12/2023 22:27:02

****Print specific timezone**
Home timezone: America/Los_Angeles
DateTime at home: 02/12/2023 14:27:02

****Culture-specific dates**
Current: 02/12/2023
English (United States) -- en-US:
2/12/2023 10:27:02 PM
2/12/2023
10:27 PM
English (Canada) -- en-CA:
2023-02-12 10:27:02 p.m.
2023-02-12
10:27 p.m.
Croatian (Croatia) -- hr-HR:
12. 02. 2023. 22:27:02
12. 02. 2023.
22:27
Korean (South Korea) -- ko-KR:
2023. 2. 12. 오후 10:27:02
2023. 2. 12.
오후 10:27
Portuguese (Brazil) -- pt-BR:
12/02/2023 22:27:02
12/02/2023
22:27
Chinese (China) -- zh-CN:
2023/2/12 下午10:27:02
2023/2/12
下午10:27

****Culture-specific currency:**
Current: ¤1,337.00
en-US: $1,337.00
en-CA: $1,337.00
hr-HR: 1.337,00 HRK
ko-KR: ₩1,337
pt-BR: R$ 1.337,00
zh-CN: ¥1,337.00
```
