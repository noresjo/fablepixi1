
// No references to PIXI here
module Hex
open System

let flatHexVertex (x,y) size i =
    let angleDegrees = 60 * i
    let angleRadians = Math.PI * float angleDegrees / 180.
    let rx = x + size * cos angleRadians
    let ry = y + size * sin angleRadians
    (rx, ry)

let flatUnitHexagon size =
    [0..5] |> List.map (flatHexVertex (0.,0.) size)
