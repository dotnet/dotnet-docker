# Enable health checks

ASP.NET Core includes [health check middleware](https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks) to enable other systems to determine the health of an app. For example, Docker and Kubernetes can be directed to use the ASP.NET Core `healthz` endpoint.

## Docker `HEALTHCHECK`

The [`HEALTHCHECK`](https://docs.docker.com/engine/reference/builder/#healthcheck) directive is used in the [`Dockerfile.alpine`](aspnetapp/Dockerfile.alpine) and [`Dockerfile.nanoserver`](aspnetapp/Dockerfile.nanoserver) samples.

Use the following commands to build the Alpine sample.

```bash
$ docker build --pull -t aspnetapp -f Dockerfile.alpine .
$ docker run --rm -it -p 8000:80 aspnetapp
```

In another terminal:

```bash
$ docker ps
CONTAINER ID   IMAGE       COMMAND         CREATED         STATUS                            PORTS                  NAMES
b143cf4ac0d1   aspnetapp   "./aspnetapp"   8 seconds ago   Up 7 seconds (health: starting)   0.0.0.0:8000->80/tcp   fervent_lichterman
```

After 30s, the status should transition to "healthy" from "health: starting".

You can also look at health status with `docker inspect`. The following pattern uses `jq`, which makes it much easier to drill in on the interesting data.

```bash
$ docker inspect b143cf4ac0d1 | jq .[-1].State.Health
{
  "Status": "healthy",
  "FailingStreak": 0,
  "Log": [
    {
      "Start": "2023-01-26T23:39:06.424631566Z",
      "End": "2023-01-26T23:39:06.589344994Z",
      "ExitCode": 0,
      "Output": "Healthy"
    },
    {
      "Start": "2023-01-26T23:39:36.597795818Z",
      "End": "2023-01-26T23:39:36.70857373Z",
      "ExitCode": 0,
      "Output": "Healthy"
    }
  ]
}
```

The same thing can be accomplished with PowerShell.

```powershell
> $healthLog = docker inspect 92648775bce8 | ConvertFrom-Json
> $healthLog[0].State.Health.Log

Start                             End                               ExitCode Output
-----                             ---                               -------- ------
2023-01-28T10:14:54.589686-08:00  2023-01-28T10:14:54.6137922-08:00        0 Healthy
2023-01-28T10:15:24.6264335-08:00 2023-01-28T10:15:24.6602762-08:00        0 Healthy
2023-01-28T10:15:54.6766598-08:00 2023-01-28T10:15:54.703489-08:00         0 Healthy
2023-01-28T10:16:24.7192354-08:00 2023-01-28T10:16:24.74409-08:00          0 Healthy
2023-01-28T10:16:54.7499988-08:00 2023-01-28T10:16:54.7750448-08:00        0 Healthy
```
