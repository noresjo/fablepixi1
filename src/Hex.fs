
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

let flatHexGrid sizex sizey size =
    let width = float (size * 2)
    let height = float size * sqrt 3.
    let func (row : int ) (column : int) =
        let centerx = float row * width * 3./4.
        let centery = float column * height + float (row % 2) * height /2.0
        (centerx, centery)

    [ for j in 1 .. sizex -> [ for i in 1..sizey -> func j i]]
    |> List.concat
