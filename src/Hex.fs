
// No references to PIXI here
module Hex
open System

let SCALE = 30.
let SQRT3 = sqrt(3.)

type CubeCoord<'T> = 
    { a: 'T
      b: 'T
      c: 'T
    }
type FloatCubeCoord = CubeCoord<float>

let IsValid cubeCoord =
    0 = cubeCoord.a + cubeCoord.b + cubeCoord.c

let cubeRound (cube : FloatCubeCoord) =
    let mutable rx = round cube.a
    let mutable ry = round cube.b
    let mutable rz = round cube.c

    let xDiff = abs(rx - cube.a)
    let yDiff = abs(ry - cube.b)
    let zDiff = abs(rz - cube.c)

    if xDiff > yDiff && xDiff > zDiff then
        rx <- -ry-rz
    else if yDiff > zDiff then
        ry <- -rx-rz
    else
        rz <- -rx-ry

    { a=int rx; b=int ry; c=int rz }

type AxialCoord<'T> =
    { q: 'T
      r: 'T
    }

let axialCoord q r =
    {q=q;r=r}

let axialAdd i j =
    {q=i.q+j.q;r=i.r+j.r}

type FloatAxialCoord = AxialCoord<float>

let axialCoordToPixel hex =         
    let x = SCALE * (     3./2. * float hex.q                    )
    let y = SCALE * ( SQRT3/2. * float hex.q  +  SQRT3 * float hex.r )
    (x,y)

let cubeCoordToAxial<'T> (cube : CubeCoord<'T>) =
    let q = cube.a
    let r = cube.c
    {r=r;q=q}

let inline axialCoordToCube (ax) =
    let fx = ax.q
    let fz = ax.r
    let fy =  -fx-fz
    {a = fx; b=fy; c=fz}    

let axialCoordcubeRound a =
     cubeRound (axialCoordToCube a) |> cubeCoordToAxial

let twoDCoordToAxial x y =
    let q = ( 2./3. * x                        ) / SCALE
    let r = (-1./3. * x  +  SQRT3/3. * y) / SCALE
    axialCoordcubeRound {q = q; r = r}

let inline cubeAdd i j =
    {
        a=i.a+j.a
        b=i.b+j.c
        c=i.c+j.c
    }

module Cube =
    let Origo =  { a = 0; b = 0; c =  0 }
    let Up = { a = 0; b = -1; c =  1 }
    let UpRight = { a = 1; b = 0; c =  1 }
    let DownRight = { a = 1; b = 1; c =  0 }
    let Down = { a = 0; b = 1; c =  -1 }
    let DownLeft = { a = -1; b = 0; c =  -1 }
    let UpLeft = { a = -1; b = -1; c =  0 }

module Axial =
    let Origo =  { q = 0; r = 0}
    let Up = { q = 0; r = -1}
    let UpRight = { q = 1; r = -1}
    let DownRight = { q = 1; r = 0}
    let Down = { q = 0; r = 1}
    let DownLeft = { q = -1; r = 1}
    let UpLeft = { q = -1; r = 0}


let hexVectorValid hv =
    0 = hv.a + hv.b + hv.c

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

let inline addTuple (x1,y1) (x2, y2) =
    (x1+x2,y1+y2)

let sameFloat f1 f2 =
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


let flatHexInRectangleCoordinates (row : int ) (column : int) =
    let centerx = float row * CurrentGridMetris.Width * 3./4.
    let centery = float column * CurrentGridMetris.Height + float (row % 2) * CurrentGridMetris.Height /2.0  - CurrentGridMetris.Height
    (centerx, centery)

let flatHexGridCoordinates sizex sizey size =
    [for j in 1 .. sizex -> [ for i in 1..sizey -> flatHexInRectangleCoordinates j i]]
    |> List.concat


let flatHexCircleGridAt radius location = 
    // var results = []
    // for each -N ≤ x ≤ +N:
    //     for each max(-N, -x-N) ≤ y ≤ min(+N, -x+N):
    //         var z = -x-y
    //         results.append(cube_add(center, Cube(x, y, z)))
    let range = [-radius..radius]
    let cubeLocation = axialCoordToCube location
    range |> List.collect (fun x -> 
        let lower = max -(radius) -(x+radius)
        let upper = min radius (radius-x)

        // printfn "x %i lower %i upper %i" x lower upper
        let result = [ for y in lower..upper -> { a=x;b=y;c= -(x+y)} ]
        // printfn "%A" result
        result
    ) |> List.map ((cubeAdd cubeLocation) >> cubeCoordToAxial)

let flatHexCircleGrid radius =
    flatHexCircleGridAt radius Axial.Origo
