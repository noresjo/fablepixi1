module AmazingApp

open Browser.Dom
open System
open Fable.Pixi
let PIXI = PIXI.pixi

let app = PIXI.Application.Create()
document.body.appendChild app.view |> ignore

let img = PIXI.Sprite.from "./fable.ico"
app.stage.addChild img |> ignore

let flatHexVertex (centerPoint : PIXI.Point) size i =
    let angleDegrees = 60 * i
    let angleRadians = Math.PI * float angleDegrees / 180.
    let x = centerPoint.x + size * cos angleRadians
    let y = centerPoint.y + size * sin angleRadians
    PIXI.Point.Create(x, y)

let flatUnitHexagon size =
    [0..5] |> List.map (flatHexVertex (PIXI.Point.Create(0., 0.)) size)


let createFlatHexagonGraphics size =
  PIXI.Graphics
    .Create()
    .lineStyle(color = (float)0x564534, width = 10.)
    .drawPolygon(Fable.Core.U3.Case2 (ResizeArray<PIXI.Point> (flatUnitHexagon size)))

let hex = 
  let result = createFlatHexagonGraphics 30.
  result.x <- 30.
  result.y <- 30.
  result
  
app.stage.addChild hex|> ignore