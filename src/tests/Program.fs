module Program
open Expecto
open App.Tests

[<EntryPoint>]
let main args =
  runTestsWithArgs defaultConfig args tests