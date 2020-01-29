module PixiHal

open Hex
open Fable.Pixi
open Fable.Core.JsInterop

let PIXI = Fable.Pixi.PIXI.pixi

let createFlatHexagonGraphics size =
 
  let hex = flatUnitHexagon size
  let pixiPointsHex = hex |> List.map (fun (x,y) -> PIXI.Point.Create(x = x, y = y ))
  let castHex = ResizeArray<PIXI.Point> pixiPointsHex
  
  PIXI.Graphics
    .Create()
    .lineStyle(color = (float)0x564534, width = 4., alpha = 0.3)
    .drawPolygon(Fable.Core.U3.Case2 castHex)


let createApplication =
  let options  = jsOptions<PIXI.ApplicationStaticOptions>(fun x ->
      x.antialias <- Some(true)
      )

  PIXI.Application.Create(options)
