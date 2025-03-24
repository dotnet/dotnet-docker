# OTLP Test Listener

A small server that implements an OTLP endpoint.

Point an application at it and it will collect the OTLP output and generate stats on what it saw

Call /report to get a json response with the stats. For example:

``` JSON
{
  "SpanIdCount": 41,
  "LogMessageCount": 10,
  "MetricNames": [
    "process.runtime.dotnet.gc.collections.count",
    "process.runtime.dotnet.gc.objects.size",
    "process.runtime.dotnet.gc.allocations.size",
    "process.runtime.dotnet.gc.committed_memory.size",
    "process.runtime.dotnet.gc.heap.size",
    "process.runtime.dotnet.gc.heap.fragmentation.size",
    "process.runtime.dotnet.gc.duration",
    "process.runtime.dotnet.jit.il_compiled.size",
    "process.runtime.dotnet.jit.methods_compiled.count",
    "process.runtime.dotnet.jit.compilation_time",
    "process.runtime.dotnet.monitor.lock_contention.count",
    "process.runtime.dotnet.thread_pool.threads.count",
    "process.runtime.dotnet.thread_pool.completed_items.count",
    "process.runtime.dotnet.thread_pool.queue.length",
    "process.runtime.dotnet.timer.count",
    "process.runtime.dotnet.assemblies.count",
    "dns.lookup.duration",
    "http.client.active_requests",
    "http.client.open_connections",
    "http.client.request.time_in_queue",
    "http.client.request.duration",
    "kestrel.active_connections",
    "kestrel.queued_connections",
    "kestrel.queued_requests",
    "kestrel.tls_handshake.duration",
    "kestrel.active_tls_handshakes",
    "http.server.active_requests",
    "http.server.request.duration",
    "aspnetcore.routing.match_attempts",
    "signalr.server.active_connections",
    "process.runtime.dotnet.exceptions.count",
    "signalr.server.connection.duration"
  ],
  "ResourceNames": [
    "webfrontend"
  ],
  "TraceIds": [
    "db56809d1fb8bcac04b90e553987b75e",
    "cdd179788682bce95bbd9dac7546756a",
    "1ad4c4f4c7397fed1da403bb3aaae5da",
    "8daee78be4d5a4680258deeef42d5de9",
    "ab9e4aa38aad08ba79abed3804b2224f",
    "f3c57e403e821d08470e3d3e1f803508",
    "5ec6e87f3f75ce0c4b0be6c50362929c",
    "0d5d632101714d8fd29399e71aca7e7b",
    "e3163da71a724718fa1a5419ae1d0d55",
    "9583c20cb576050111733130e6914970",
    "6e623af4d3090b5f0cf0954c06182211",
    "2a770fbe654c5f605a03b054b239b6d3",
    "5d3a9b5cda00ef68828e85238e906cf1",
    "518f7be3440418dbbf91210825c2e963",
    "66b5d2b6df92ae3fea268d7c80d8ba38",
    "7f08a437b8c51b39eb6f4dedacde17d3",
    "ebcae7e19a54a3959bada9f054822ecd",
    "307b55c66f631e94203c0d17fd13f46d",
    "fb8b4ee59bc42883a0a3a7b3b50c936f",
    "ccb95e82fde12dc560905f3fd4bddedd",
    "5845865ef16eda10b262ada83164a95e",
    "5223bdda12e4cac786b2735a56a60840",
    "434cb6b69651252bb0db7d2762e26d7e",
    "ae0afd9aba85ae46a7b634eacb8d27fe",
    "c04cfd5f00a429573933994a34c0f47e",
    "43f3e198e6c1445b2f8e6d0d3697ecfc",
    "b74acff1e0f346b0d68920433bde3dd0",
    "2e41608348eb69ba5d9df83d4c1b457e",
    "b1a3eab6982538ca8870ac4d79b61eb6",
    "0bd4c8dee515f755170654e15a7733fc"
  ],
  "MetricNameCount": 32,
  "TraceIdCount": 30
}
```
