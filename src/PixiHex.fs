module PixiHex

open Hex
open PixiHal
open Fable.Pixi
open Fable.Core

let rows = Constants.GridRows
let columns = Constants.GridColumns
let even = (float (columns) % 2.)
let oddColumns = float (columns /2) + even
let evenColumns = float (columns / 2) 
let flatBoardHeight = (float rows + 0.5) * Hex.CurrentGridMetris.Height 
let flatBoardWidth = 
    CurrentGridMetris.Width * oddColumns +
    CurrentGridMetris.HalfWidth * 1. * evenColumns +
    CurrentGridMetris.HalfWidth * 0.5 * (1. - even)


let boardRectangle =
  PIXI.Rectangle.Create(
    CurrentGridMetris.HalfWidth/2.,
    -CurrentGridMetris.HalfHeight,
    flatBoardWidth,
    flatBoardHeight
  )

let createFlatHexagonGraphics size =

  let hex = flatUnitHexagonVertices Hex.SCALE
  let pixiPointsHex = hex |> List.map (fun (x,y) -> PIXI.Point.Create(x = x, y = y ))
  let castHex = ResizeArray<PIXI.Point> pixiPointsHex
  
  PIXI.Graphics
    .Create()
    .lineStyle(color = Constants.HexagonColor, width = 1., alpha = 1.)
    .drawPolygon(Fable.Core.U3.Case2 castHex)


let CreateHexBoard =
  let graphics = PIXI.Graphics.Create()
 
  graphics
    .beginFill(Constants.BoardBackground)
    .lineStyle(color = Constants.BoardBorderColor, width = 1.0, alpha = Constants.GridAlpha)
    .drawRect(boardRectangle.x, boardRectangle.y, boardRectangle.width, boardRectangle.height)
    .endFill()
    .lineStyle(color = Constants.GridColor, width = 1.0, alpha = Constants.GridAlpha)
    .hitArea <- Fable.Core.U5.Case1 boardRectangle
  
  graphics

let CreateLineSegmentHexGrid graphics =
  let lines = 
    Hex.flatHexGridCoordinates columns rows (int Hex.SCALE)
    |> List.collect (fun (hexx,hexy) -> 
    (flatUnitHexagonLines Hex.SCALE)
    |> List.map (fun ((x1,y1),(x2,y2)) -> (x1+hexx,y1+hexy),(x2+hexx,y2+hexy)))

  lines |> List.distinctBy (fun ((x1,y1),(x2,y2)) -> (1000. * (x1+y1+x2+y2)) |> round)
 
  |> List.map (fun (a,b) -> DrawLineSegmentOnGrapics graphics a b)
  |> ignore

  graphics

let drawRect (graphics : PIXI.Graphics) rect =
  graphics
    .lineStyle(color = Constants.BoardBorderColor, width = 1.0, alpha = Constants.GridAlpha)
    .drawShape(U5.Case4 rect)

let CreateBoardBorder (graphics : PIXI.Graphics) =
  drawRect graphics boardRectangle

let hexAt (parent : Fable.Pixi.PIXI.Container) (sx, sy) = 
  let result = createFlatHexagonGraphics()
  result.x <- sx
  result.y <- sy
  parent.addChild result

let hexAtAxial (parent : Fable.Pixi.PIXI.Container) location =
  hexAt parent (location |> axialCoordToPixel)
