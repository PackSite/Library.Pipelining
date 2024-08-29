```
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4037/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X3D, 1 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.400
  [Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```

# Invocation Performance Benchmark

| Method                       | Mean      | Error     | StdDev    | Ratio | RatioSD | Rank |
|----------------------------- |----------:|----------:|----------:|------:|--------:|-----:|
| 'Steps.Count == 1'           |  13.80 us |  0.272 us |  0.291 us |  0.38 |    0.01 |    1 |
| 'Steps.Count == 2'           |  16.79 us |  0.321 us |  0.300 us |  0.46 |    0.01 |    2 |
| 'Steps.Count == 5'           |  32.77 us |  0.640 us |  0.711 us |  0.89 |    0.02 |    3 |
| 'Loop simulation (i == 100)' |  36.67 us |  0.703 us |  0.691 us |  1.00 |    0.03 |    4 |
| 'Steps.Count == 10'          |  67.48 us |  1.604 us |  4.730 us |  1.84 |    0.13 |    5 |
| 'Steps.Count == 50'          | 307.87 us |  6.153 us |  7.781 us |  8.40 |    0.26 |    6 |
| 'Steps.Count == 100'         | 600.16 us | 11.493 us | 15.343 us | 16.37 |    0.51 |    7 |