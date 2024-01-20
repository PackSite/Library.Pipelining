```
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X3D, 1 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```

# Invocation Performance Benchmark

| Method                       | Mean      | Error    | StdDev   | Ratio | RatioSD | Rank |
|----------------------------- |----------:|---------:|---------:|------:|--------:|-----:|
| 'Steps.Count == 1'           |  12.02 us | 0.148 us | 0.124 us |  0.33 |    0.01 |    1 |
| 'Steps.Count == 2'           |  15.43 us | 0.164 us | 0.153 us |  0.42 |    0.01 |    2 |
| 'Steps.Count == 5'           |  29.47 us | 0.202 us | 0.179 us |  0.80 |    0.01 |    3 |
| 'Loop simulation (i == 100)' |  36.82 us | 0.447 us | 0.418 us |  1.00 |    0.00 |    4 |
| 'Steps.Count == 10'          |  57.59 us | 0.417 us | 0.390 us |  1.56 |    0.02 |    5 |
| 'Steps.Count == 50'          | 276.01 us | 2.743 us | 2.432 us |  7.50 |    0.12 |    6 |
| 'Steps.Count == 100'         | 563.48 us | 2.442 us | 2.165 us | 15.31 |    0.15 |    7 |