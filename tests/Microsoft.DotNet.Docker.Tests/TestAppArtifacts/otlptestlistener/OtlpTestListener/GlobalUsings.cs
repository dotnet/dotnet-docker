global using System.Collections;
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Text;
global using System.Web;

global using Google.Protobuf.Collections;

global using Grpc.Core;

global using OpenTelemetry.Proto.Collector.Logs.V1;
global using OpenTelemetry.Proto.Collector.Metrics.V1;
global using OpenTelemetry.Proto.Collector.Trace.V1;
global using OpenTelemetry.Proto.Common.V1;
global using OpenTelemetry.Proto.Logs.V1;
global using OpenTelemetry.Proto.Metrics.V1;
global using OpenTelemetry.Proto.Resource.V1;

global using OtlpTestListener;
global using OtlpTestListener.DataModel;
global using OtlpTestListener.Extensions;

global using Otel = OpenTelemetry.Proto.Trace.V1;

global using Microsoft.Extensions.Logging;
