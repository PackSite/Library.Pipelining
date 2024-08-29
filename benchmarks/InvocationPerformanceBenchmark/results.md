```
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4037/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X3D, 1 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.400
  [Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```

# Invocation Performance Benchmark

| Method                      | Mean      | Error    | StdDev   | Ratio | RatioSD | Rank |
|---------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
| 'Steps.Count == 1'          |  11.29 us | 0.226 us | 0.590 us |  0.65 |    0.03 |    1 |
| 'Steps.Count == 2'          |  12.31 us | 0.131 us | 0.116 us |  0.71 |    0.01 |    2 |
| 'Loop simulation (i == 50)' |  17.29 us | 0.137 us | 0.128 us |  1.00 |    0.01 |    3 |
| 'Steps.Count == 5'          |  27.01 us | 0.274 us | 0.257 us |  1.56 |    0.02 |    4 |
| 'Steps.Count == 10'         |  47.34 us | 0.470 us | 0.439 us |  2.74 |    0.03 |    5 |
| 'Steps.Count == 50'         | 259.51 us | 2.564 us | 2.399 us | 15.01 |    0.17 |    6 |

> Every pipeline was invoked 100 times - so for pipeline with 5 steps, a 500 steps were executed.
