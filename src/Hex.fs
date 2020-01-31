
// No references to PIXI here
module Hex
open System
open System.Numerics


let eqMargin = 0.001

let sameFloat (f1:float) (f2:float) =
    (abs(f1-f2)) < eqMargin

let samePoint (x1,y1) (x2, y2) =
    (sameFloat x1 x2) && (sameFloat y1 y2)

let SameLineSegment p1 p2 p3 p4 =
    ((samePoint p1 p3) || (samePoint p1 p4)) &&
    ((samePoint p3 p3) || (samePoint p3 p4))

let pointDifference ((x1:float),(y1:float)) (x2, y2) =
    (x1-x2,y1-y2)

let dotProduct2d ((x1:float),(y1:float)) (x2, y2) =
    x1*x2 + y1*y2

let vectorLength p1 =
    sqrt(dotProduct2d p1 p1)

let distance p1 p2  =
    vectorLength(pointDifference p1 p2)

let distancePointToLineSegment p1 lp1 lp2 =
    let v = pointDifference lp2 p1
    let w = pointDifference p1 lp1

    let c1 = dotProduct2d w v
    let c2 = dotProduct2d v v

    if c1 <= 0. then
        distance p1 lp1
    else if c2 <= c1 then
        distance p1 lp2
    else
        let b = c1 / c2
        let pb = (fst p1 + b*fst v, snd p1 + b*snd v)
        distance p1 pb


let flatHexVertex (x,y) size i =
    let angleDegrees = 60 * i
    let angleRadians = Math.PI * float angleDegrees / 180.
    let rx = x + size * cos angleRadians
    let ry = y + size * sin angleRadians
    (rx, ry)

let flatUnitHexagonVertices size =
    [0..5] |> List.map (flatHexVertex (0.,0.) size)

let flatUnitHexagonLines size =
    [0..5] 
    |> List.map (fun index -> ((flatHexVertex (0.,0.) size index), (flatHexVertex (0.,0.) size (index + 1 % 6))))

let flatHexGridCoordinates sizex sizey size =
    let width = float (size * 2)
    let height = float size * sqrt 3.
    let func (row : int ) (column : int) =
        let centerx = float row * width * 3./4.
        let centery = float column * height + float (row % 2) * height /2.0
        (centerx, centery)

    [ for j in 1 .. sizex -> [ for i in 1..sizey -> func j i]]
    |> List.concat
