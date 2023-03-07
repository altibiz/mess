using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
  "CodeQuality",
  "xUnit1013:Public method should be marked as test",
  Justification = "Deconstruct on record class test types",
  Scope = "namespaceanddescendants",
  Target = "~N:Mess.Test"
)]
