# Library.Pipelining

[![CI](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml/badge.svg)](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml)
[![Coverage](https://codecov.io/gh/PackSite/Library.Pipelining/branch/main/graph/badge.svg?token=59vj2CRtyN)](https://codecov.io/gh/PackSite/Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.svg?label=Pipelining)](https://nuget.org/packages/PackSite.Library.Pipelining)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Abstractions.svg?label=Pipelining.Abstractions)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Abstractions.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Configuration.svg?label=Pipelining.Configuration)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Configuration.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)

**PackSite.Library.Pipelining** is a set of **.NET 8** compatible libraries for building modern **ASP.NET Core** middleware-like data pipelines.

## Features
  
  - Uses `Microsoft.Extensions.*` to support a modular library design (can also be used without Generic Host but with limited features).
  - Support for asynchronous processing model.
  - Transient, Scoped, and Singleton pipeline lifetime.
  - Pipelines distinguished by custom names or pipeline argument data type.
  - Fluent pipeline builder (for defining pipeline name, description, and steps).
  - Highly configurable  steps order using `Add`, `Insert`, `InsertBefore`, `InsertAfter` methods.
  - Dynamic sub-pipelines.
  - Generic and object-based steps.
  - Code-first pipeline definitions during Generic Host start and at runtime through `IPipelineCollection`.
  - Optional configuration-based pipelines definitions using `IOptions` with reloading.
  - Pipeline counters: executions count (successful and failure) and speed (microsecond resolution).
  - Very fast step execution (see Benchmarks section).

## Examples

See [Examples folder](https://github.com/PackSite/Library.Pipelining/tree/main/examples) for all library usage examples.

## Benchmarks

```
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3693/22H2/2022Update)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```

| Method                       | Mean        | Error    | StdDev   | Ratio | RatioSD | Rank |
|----------------------------- |------------:|---------:|---------:|------:|--------:|-----:|
| 'Steps.Count == 1'           |    20.13 us | 0.401 us | 0.961 us |  0.35 |    0.02 |    1 |
| 'Steps.Count == 2'           |    26.54 us | 0.500 us | 0.650 us |  0.43 |    0.01 |    2 |
| 'Steps.Count == 5'           |    52.33 us | 1.037 us | 1.520 us |  0.87 |    0.03 |    3 |
| 'Loop simulation (i == 100)' |    61.08 us | 0.141 us | 0.110 us |  1.00 |    0.00 |    4 |
| 'Steps.Count == 10'          |    96.50 us | 1.840 us | 1.536 us |  1.58 |    0.02 |    5 |
| 'Steps.Count == 50'          |   501.68 us | 8.753 us | 8.188 us |  8.24 |    0.13 |    6 |
| 'Steps.Count == 100'         | 1,049.00 us | 8.896 us | 6.946 us | 17.18 |    0.12 |    7 |

> Every pipeline was invoked 100 times - so for pipeline with 5 steps, a 500 steps were executed.
