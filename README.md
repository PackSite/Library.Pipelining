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
BenchmarkDotNet v0.13.7, Windows 10 (10.0.19045.3448/22H2/2022Update)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
```

|                       Method |        Mean |     Error |    StdDev | Ratio | RatioSD | Rank |
|----------------------------- |------------:|----------:|----------:|------:|--------:|-----:|
|           'Steps.Count == 1' |    24.04 us |  0.479 us |  0.911 us |  0.39 |    0.01 |    1 |
|           'Steps.Count == 2' |    31.67 us |  0.483 us |  0.451 us |  0.51 |    0.01 |    2 |
| 'Loop simulation (i == 100)' |    62.66 us |  1.157 us |  1.083 us |  1.00 |    0.00 |    3 |
|           'Steps.Count == 5' |    67.43 us |  1.327 us |  2.524 us |  1.09 |    0.04 |    4 |
|          'Steps.Count == 10' |   127.96 us |  2.365 us |  5.339 us |  2.05 |    0.09 |    5 |
|          'Steps.Count == 50' |   643.48 us | 12.651 us | 12.425 us | 10.27 |    0.21 |    6 |
|         'Steps.Count == 100' | 1,333.14 us | 15.208 us | 11.874 us | 21.35 |    0.47 |    7 |

> Every pipeline was invoked 100 times - so for pipeline with 5 steps, a 500 steps were executed.
