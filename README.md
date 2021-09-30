# Library.Pipelining

[![CI](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml/badge.svg)](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml)
[![Coverage](https://codecov.io/gh/PackSite/Library.Pipelining/branch/main/graph/badge.svg?token=59vj2CRtyN)](https://codecov.io/gh/PackSite/Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.svg?label=Pipelining)](https://nuget.org/packages/PackSite.Library.Pipelining)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Abstractions.svg?label=Pipelining.Abstractions)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Abstractions.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Configuration.svg?label=Pipelining.Configuration)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Configuration.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)

**PackSite.Library.Pipelining** is a set of **.NET Standard 2.1** and **.NET 5** libraries for building modern **ASP.NET Core** middleware-like data pipelines.

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
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.401
  [Host]     : .NET 5.0.10 (5.0.1021.41214), X64 RyuJIT
  DefaultJob : .NET 5.0.10 (5.0.1021.41214), X64 RyuJIT
```

|               Method |       Mean |    Error |   StdDev | Ratio | RatioSD | Rank |
|--------------------- |-----------:|---------:|---------:|------:|--------:|-----:|
|    'Loop simulation' |   306.2 ns |  0.97 ns |  0.91 ns |  1.00 |    0.00 |    1 |
|   'Steps.Count == 1' | 1,779.0 ns | 14.27 ns | 12.65 ns |  5.81 |    0.04 |    2 |
|   'Steps.Count == 2' | 2,024.5 ns | 24.62 ns | 20.56 ns |  6.61 |    0.06 |    3 |
|  'Steps.Count == 10' | 2,103.0 ns |  6.48 ns |  5.41 ns |  6.87 |    0.03 |    4 |
|   'Steps.Count == 5' | 2,110.9 ns | 13.04 ns | 10.89 ns |  6.90 |    0.04 |    4 |
| 'Steps.Count == 100' | 2,253.0 ns | 10.74 ns |  9.52 ns |  7.36 |    0.04 |    5 |