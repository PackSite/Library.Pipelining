# Library.Pipelining

[![CI](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml/badge.svg)](https://github.com/PackSite/Library.Pipelining/actions/workflows/CI.yml)
[![Coverage](https://codecov.io/gh/PackSite/Library.Pipelining/branch/main/graph/badge.svg?token=59vj2CRtyN)](https://codecov.io/gh/PackSite/Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.svg?label=Pipelining)](https://nuget.org/packages/PackSite.Library.Pipelining)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Abstractions.svg?label=Pipelining.Abstractions)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Abstractions.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Abstractions)

[![Version](https://img.shields.io/nuget/v/PackSite.Library.Pipelining.Configuration.svg?label=Pipelining.Configuration)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)
[![Downloads](https://img.shields.io/nuget/dt/PackSite.Library.Pipelining.Configuration.svg?label=)](https://nuget.org/packages/PackSite.Library.Pipelining.Configuration)

**PackSite.Library.Pipelining** is a set of **.NET 8** compatible libraries for building modern **ASP.NET Core** middleware-like data pipelines that are fully asynchronous.

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
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4037/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X3D, 1 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.400
  [Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```

| Method                      | Mean      | Error    | StdDev   | Ratio | RatioSD | Rank |
|---------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
| 'Steps.Count == 1'          |  11.29 us | 0.226 us | 0.590 us |  0.65 |    0.03 |    1 |
| 'Steps.Count == 2'          |  12.31 us | 0.131 us | 0.116 us |  0.71 |    0.01 |    2 |
| 'Loop simulation (i == 50)' |  17.29 us | 0.137 us | 0.128 us |  1.00 |    0.01 |    3 |
| 'Steps.Count == 5'          |  27.01 us | 0.274 us | 0.257 us |  1.56 |    0.02 |    4 |
| 'Steps.Count == 10'         |  47.34 us | 0.470 us | 0.439 us |  2.74 |    0.03 |    5 |
| 'Steps.Count == 50'         | 259.51 us | 2.564 us | 2.399 us | 15.01 |    0.17 |    6 |

> Every pipeline was invoked 100 times - so for pipeline with 5 steps, a 500 steps were executed.
