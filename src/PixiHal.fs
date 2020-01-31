module PixiHal

open Hex
open Fable.Pixi
open Fable.Core.JsInterop

let PIXI = Fable.Pixi.PIXI.pixi

let TupleToPoint (x, y) =
  (PIXI.Point.Create(x = x, y = y))

let DrawLineSegmentOnGrapics (graphics : PIXI.Graphics) (x1,y1) (x2,y2) =
  graphics.moveTo(x1,y1).lineTo(x2,y2)

let createFlatHexagonGraphics size =

  let hex = flatUnitHexagonVertices size
  let pixiPointsHex = hex |> List.map (fun (x,y) -> PIXI.Point.Create(x = x, y = y ))
  let castHex = ResizeArray<PIXI.Point> pixiPointsHex
  
  PIXI.Graphics
    .Create()
    .lineStyle(color = (float)0x564534, width = 4., alpha = 0.3)
    .drawPolygon(Fable.Core.U3.Case2 castHex)

let CreateLineSegmentHexGrid  =
  let graphics = PIXI.Graphics.Create().lineStyle(color = (float)0x664422, width = 2.0, alpha = 1.)

  let lines = 
    Hex.flatHexGridCoordinates 10 5 40
    |> List.collect (fun (hexx,hexy) -> 
    (flatUnitHexagonLines 40.)
    |> List.map (fun ((x1,y1),(x2,y2)) -> (x1+hexx,y1+hexy),(x2+hexx,y2+hexy)))

  lines |> List.distinctBy (fun ((x1,y1),(x2,y2)) -> x1+y1+x2+y2 |> round)
 
  |> List.map (fun (a,b) -> DrawLineSegmentOnGrapics graphics a b)
  |> ignore

  graphics

let createApplication =
  let options  = jsOptions<PIXI.ApplicationStaticOptions>(fun x ->
      x.antialias <- Some(true)
      )

  PIXI.Application.Create(options)
