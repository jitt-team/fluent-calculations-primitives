using BenchmarkDotNet.Running;
using Fluent.Calculations.Primitives.Tests.Benchmarks;

BenchmarkRunner.Run<CalculationBenchmarks>();
Console.ReadLine();
