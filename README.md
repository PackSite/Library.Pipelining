# Library.Pipelining

[![CI](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml/badge.svg)](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml)
[![Coverage](https://codecov.io/gh/PackSite/Library.Pipelining/branch/main/graph/badge.svg?token=59vj2CRtyN)](https://codecov.io/gh/PackSite/Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.svg?label=Pipelining)](https://nuget.org/packages/PackSite.Library.Pipelining)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Abstractions.svg?label=Pipelining.Abstractions)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Abstractions.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Configuration.svg?label=Pipelining.Configuration)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Configuration.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)

**PackSite.Library.Pipelining** is a set of **.NET Standard 2.1**, **.NET 5**, and **.NET 6** compatible libraries for building modern **ASP.NET Core** middleware-like data pipelines.

## Features
  
  - Uses `Microsoft.Extensions.*` to suppoort a modular library design (can also be used without Generic Host but with limited features).
  - Support for asynchronous processing model.
  - Transient, Scoped, and Singleton pipeline lifetime.
  - Pipelines distinguised by custom names or pipeline argument data type.
  - Fluent pipeline builder (for defining pipeline name, description, and steps).
  - Dynamic subpipelines.
  - Generic and object-based steps.
  - Code-first pipeline definitions during Generic Host start and at runtime through `IPipelineCollection`.
  - Optional configuration-based pipelines definitions using `IOptions` with reloading.
  - Pipeline counters: executions count (successful and failure) and speed (resolution in microseconds).
  - Very fast step execution (see Benchmarks section).

## Examples

See [Examples folder](https://github.com/PackSite/Library.Pipelining/tree/main/examples) for all library usage examples.

## Benchmarks

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
```

|               Method |       Mean |   Error |  StdDev | Ratio | Rank |
|--------------------- |-----------:|--------:|--------:|------:|-----:|
|    'Loop simulation' |   304.0 ns | 0.20 ns | 0.18 ns |  1.00 |    1 |
|   'Steps.Count == 1' | 1,782.0 ns | 3.70 ns | 2.89 ns |  5.86 |    2 |
|   'Steps.Count == 2' | 1,870.6 ns | 1.69 ns | 1.32 ns |  6.15 |    3 |
|  'Steps.Count == 10' | 2,128.0 ns | 3.46 ns | 3.24 ns |  7.00 |    4 |
|   'Steps.Count == 5' | 2,131.5 ns | 4.33 ns | 3.84 ns |  7.01 |    4 |
| 'Steps.Count == 100' | 2,157.3 ns | 3.10 ns | 2.75 ns |  7.10 |    5 |