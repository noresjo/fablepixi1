module PixiHal

open Hex
open Fable.Pixi
open Fable.Core.JsInterop

let PIXI = Fable.Pixi.PIXI.pixi

let Constants = {|
  Background = (float 0x191970)
  GridBackground = (float 0x777777)
  GridAlpha = 1
  GridColor = (float 0x111111)
  HexagonColor = (float 0x564534)
  TextColor = (float 0xddddff)
  GridRows = 32
  GridColumns = 50
|}

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
    .lineStyle(color = Constants.HexagonColor, width = 4., alpha = 0.3)
    .drawPolygon(Fable.Core.U3.Case2 castHex)

let CreateLineSegmentHexGrid  =
  let graphics = PIXI.Graphics.Create().lineStyle(color = Constants.GridColor, width = 1.0, alpha = 1.)
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
    .beginFill(Constants.GridBackground)
    .drawRect(rectangle.x, rectangle.y, rectangle.width, rectangle.height)
    .endFill()
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

let createApplication =
  let options  = jsOptions<PIXI.ApplicationStaticOptions>(fun x ->
      x.antialias <- Some(true)
      x.backgroundColor <-Some(Constants.Background)
      )

  PIXI.Application.Create(options)

let style2 = jsOptions< PIXI.TextStyle>(fun x ->
      x.fill <- Fable.Core.U6.Case3 Constants.TextColor
      x.fontSize <- Fable.Core.U2.Case1 12.
  )
