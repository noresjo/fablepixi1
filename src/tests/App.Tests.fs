module App.Tests

open Expecto
open Hex
[<Tests>]
let tests =
  testList "flatHexCircleGrid" [
    test "flatHexCircleGrid 1" {
      let subject = flatHexCircleGrid 1
      Expect.equal subject.Length 7 "Correct length"
      Expect.equal (subject |> List.distinct).Length 7 "All hexes are unique"
      Expect.all subject (axialCoordToCube >> IsValid) "All valid"

      let unitCircle = [
        Axial.Down
        Axial.DownLeft
        Axial.DownRight
        Axial.Up
        Axial.UpLeft
        Axial.UpRight
        Axial.Origo
      ]

      Expect.isTrue (unitCircle |> List.forall (fun x -> (List.contains x subject))) " correct hexes"

    }

    test "flatHexCircleGrid 2" {
      let subject = flatHexCircleGrid 2
      Expect.equal subject.Length 19 "Correct length"
      Expect.equal (subject |> List.distinct).Length 19 "All hexes are unique"
      Expect.all subject (axialCoordToCube >> IsValid) "All valid"
    }
  ]



