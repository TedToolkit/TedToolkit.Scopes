// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

using TedToolkit.Scopes.Benchmark;

Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<ScopeRunner>();