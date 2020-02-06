module PixiHex

open Hex
open PixiHal
open Fable.Pixi


let createFlatHexagonGraphics size =

  let hex = flatUnitHexagonVertices Hex.SCALE
  let pixiPointsHex = hex |> List.map (fun (x,y) -> PIXI.Point.Create(x = x, y = y ))
  let castHex = ResizeArray<PIXI.Point> pixiPointsHex
  
  PIXI.Graphics
    .Create()
    .lineStyle(color = Constants.HexagonColor, width = 1., alpha = 1.)
    .drawPolygon(Fable.Core.U3.Case2 castHex)

let CreateLineSegmentHexGrid  =
  let graphics = PIXI.Graphics.Create()
  let rows = Constants.GridRows
  let columns = Constants.GridColumns
  let gridHeight = (float rows + 0.5) * Hex.CurrentGridMetris.Height 

  let even = (float (columns) % 2.)
  let oddColumns = float (columns /2) + even
  let evenColumns = float (columns / 2) 
  
  printfn "%f" oddColumns
  printfn "%f" evenColumns

  let gridWidth = 
    CurrentGridMetris.Width * oddColumns +
    CurrentGridMetris.HalfWidth * 1. * evenColumns +
    CurrentGridMetris.HalfWidth * 0.5 * (1. - even)
 
  let rectangle =
    PIXI.Rectangle.Create(
      0.,
      0.,
      gridWidth,
      gridHeight
    )

  graphics
//    .beginFill(Constants.GridBackground)
    .lineStyle(color = Constants.GridBorderColor, width = 1.0, alpha = Constants.GridAlpha)
//    .drawRect(rectangle.x, rectangle.y, rectangle.width, rectangle.height)
//    .endFill()
    .lineStyle(color = Constants.GridColor, width = 1.0, alpha = Constants.GridAlpha)
    .hitArea <- Fable.Core.U5.Case1 rectangle
  let lines = 
    Hex.flatHexGridCoordinates columns rows (int Hex.SCALE)
    |> List.collect (fun (hexx,hexy) -> 
    (flatUnitHexagonLines Hex.SCALE)
    |> List.map (fun ((x1,y1),(x2,y2)) -> (x1+hexx,y1+hexy),(x2+hexx,y2+hexy)))

  lines |> List.distinctBy (fun ((x1,y1),(x2,y2)) -> (1000. * (x1+y1+x2+y2)) |> round)
 
  |> List.map (fun (a,b) -> DrawLineSegmentOnGrapics graphics a b)
  |> ignore

  graphics
//    .lineStyle(color = Constants.GridBorderColor, width = 1.0, alpha = Constants.GridAlpha)
//    .drawRect(rectangle.x, rectangle.y, rectangle.width, rectangle.height)
