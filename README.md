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
BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2130/21H2/November2021Update)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.10 (6.0.1022.47605), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.10 (6.0.1022.47605), X64 RyuJIT AVX2
```

|                       Method |        Mean |     Error |    StdDev | Ratio | RatioSD | Rank |
|----------------------------- |------------:|----------:|----------:|------:|--------:|-----:|
|           'Steps.Count == 1' |    21.64 us |  0.217 us |  0.193 us |  0.36 |    0.00 |    1 |
|           'Steps.Count == 2' |    30.74 us |  0.568 us |  0.531 us |  0.51 |    0.01 |    2 |
| 'Loop simulation (i == 100)' |    60.83 us |  0.258 us |  0.228 us |  1.00 |    0.00 |    3 |
|           'Steps.Count == 5' |    63.75 us |  0.179 us |  0.140 us |  1.05 |    0.01 |    4 |
|          'Steps.Count == 10' |   119.74 us |  2.046 us |  1.914 us |  1.97 |    0.03 |    5 |
|          'Steps.Count == 50' |   597.01 us |  4.535 us |  4.242 us |  9.82 |    0.08 |    6 |
|         'Steps.Count == 100' | 1,259.79 us | 11.527 us | 10.219 us | 20.71 |    0.18 |    7 |

> Every pipeline was invoked 100 times - so for pipeline with 5 steps, a 500 steps were executed.
