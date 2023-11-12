using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Fluent.Calculations.Primitives.Tests.Benchmarks;

Summary summary = BenchmarkRunner.Run<CalculationBenchmarks>();
Console.ReadLine();
