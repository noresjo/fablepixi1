
// No references to PIXI here
module Hex
open System

let SCALE = 20.
let SQRT3 = sqrt(3.)

type CubeCoord = 
    { cx: int
      cy: int
      cz: int
    }
  with member this.IsValid =
                0 = this.cx + this.cy + this.cz

type FloatCubeCoord = 
    { fcx: float
      fcy: float
      fcz: float
    }
    with member cube.ToCubeCoord =
                let mutable rx = round cube.fcx
                let mutable ry = round cube.fcy
                let mutable rz = round cube.fcz

                let xDiff = abs(rx - cube.fcx)
                let yDiff = abs(ry - cube.fcy)
                let zDiff = abs(rz - cube.fcz)

                if xDiff > yDiff && xDiff > zDiff then
                    rx <- -ry-rz
                else if yDiff > zDiff then
                    ry <- -rx-rz
                else
                    rz <- -rx-ry

                { cx=int rx; cy=int ry; cz=int rz }

type FloatAxialCoord =
    { fq: float
      fr: float
    }


type AxialCoord =
    { q: int
      r: int
    }
    with member hex.ToTwoDCoord =         
                    let x = SCALE * (     3./2. * float hex.q                    )
                    let y = SCALE * ( SQRT3/2. * float hex.q  +  SQRT3 * float hex.r )
                    (x,y)



let cubeCoordToAxial (cube : CubeCoord) =
    let q = cube.cx
    let r = cube.cz
    {r=r;q=q}

let floatAxialCoordToCube (fax : FloatAxialCoord) =
    let fx = fax.fq
    let fz = fax.fr
    let fy = -fx-fz
    {fcx = fx; fcy=fy; fcz=fz}    

let floatAxialCoordRound (a : FloatAxialCoord) =
    (floatAxialCoordToCube a).ToCubeCoord |> cubeCoordToAxial

let twoDCoordToAxial x y =
    let q = ( 2./3. * x                        ) / SCALE
    let r = (-1./3. * x  +  SQRT3/3. * y) / SCALE
    floatAxialCoordRound {fq = q; fr = r}



let ORIGO =  { cx = 0; cy = 0; cz =  0 }
let UP = { cx = 0; cy = -1; cz =  1 }
let UP_RIGHT = { cx = 1; cy = 0; cz =  1 }
let DOWN_RIGHT = { cx = 1; cy = 1; cz =  0 }
let DOWN = { cx = 0; cy = 1; cz =  -1 }
let DOWN_LEFT = { cx = -1; cy = 0; cz =  -1 }
let UP_LEFT = { cx = -1; cy = -1; cz =  0 }

let hexVectorValid hv =
    0 = hv.cx + hv.cy + hv.cz


type HexMetrics =
    {
        HalfWidth : float
        HalfHeight : float
        Width : float
        Height : float
    }

let CurrentGridMetris = 
    {
        HalfWidth = SCALE
        HalfHeight = SCALE * SQRT3 / 2.
        Width = SCALE * 2.
        Height = SCALE * SQRT3
    }

let eqMargin = 0.001

let sameFloat (f1:float) (f2:float) =
    (abs(f1-f2)) < eqMargin

let samePoint (x1,y1) (x2, y2) =
    (sameFloat x1 x2) && (sameFloat y1 y2)

let SameLineSegment p1 p2 p3 p4 =
    ((samePoint p1 p3) || (samePoint p2 p3)) &&
    ((samePoint p3 p3) || (samePoint p2 p4))

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
    // CurrentGridMetris.HalfWidth * 0.5,
    // CurrentGridMetris.HalfHeight,
    // let width = float (size * 2)
    // let height = float size * sqrt 3.
    let func (row : int ) (column : int) =
        let centerx = float row * CurrentGridMetris.Width * 3./4.
        let centery = float column * CurrentGridMetris.Height + float (row % 2) * CurrentGridMetris.Height /2.0  - CurrentGridMetris.Height
        (centerx, centery)

    [for j in 1 .. sizex -> [ for i in 1..sizey -> func j i]]
    |> List.concat
