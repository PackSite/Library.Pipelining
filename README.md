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
  - Highly configurable  steps order using `Add`, `Insert`, `InsertBefore`, `InsertAfter` methods.
  - Dynamic subpipelines.
  - Generic and object-based steps.
  - Code-first pipeline definitions during Generic Host start and at runtime through `IPipelineCollection`.
  - Optional configuration-based pipelines definitions using `IOptions` with reloading.
  - Pipeline counters: executions count (successful and failure) and speed (microsecond resolution).
  - Very fast step execution (see Benchmarks section).

## Examples

See [Examples folder](https://github.com/PackSite/Library.Pipelining/tree/main/examples) for all library usage examples.

## Benchmarks

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
```

|                       Method |        Mean |    Error |   StdDev | Ratio | Rank |
|----------------------------- |------------:|---------:|---------:|------:|-----:|
|           'Steps.Count == 1' |    22.80 us | 0.345 us | 0.323 us |  0.34 |    1 |
|           'Steps.Count == 2' |    31.12 us | 0.169 us | 0.158 us |  0.47 |    2 |
|           'Steps.Count == 5' |    64.17 us | 0.349 us | 0.291 us |  0.97 |    3 |
| 'Loop simulation (i == 100)' |    66.33 us | 0.108 us | 0.084 us |  1.00 |    4 |
|          'Steps.Count == 10' |   119.05 us | 0.481 us | 0.402 us |  1.80 |    5 |
|          'Steps.Count == 50' |   605.81 us | 1.675 us | 1.485 us |  9.13 |    6 |
|         'Steps.Count == 100' | 1,280.27 us | 6.409 us | 5.352 us | 19.30 |    7 |

> Every pipeline was invoked 100 times - so for pipeline with 5 steps, a 500 steps were executed.
