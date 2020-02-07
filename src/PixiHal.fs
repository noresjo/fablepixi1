module PixiHal

open Hex
open Fable.Pixi
open Fable.Core.JsInterop

let PIXI = Fable.Pixi.PIXI.pixi

let Constants = {|
  Background = (float 0x193549)
  BoardBorderColor = (float 0x04080a)
  BoardBackground = (float 0x0050A4)
  GridAlpha = 1.
  TextColor = (float 0xc6ff00)
  HexagonColor = (float 0xff00c6)
  GridColor = (float 0x00c6ff)
  GridRows = 15
  GridColumns = 25
|}

type Coordinate =
  | PixiPunkt of Fable.Pixi.PIXI.Point
  | Tuple of float * float
 
let ToCoordinateString (c : Coordinate) =
  let result x y = 
    sprintf "(%i,%i)" (x |> int) (y |> int)

  match c with
  | PixiPunkt c -> result c.x c.y
  | Tuple (x,y) -> result x y


let TupleToPixiPoint (x, y) =
  (PIXI.Point.Create(x = x, y = y))

let DrawLineSegmentOnGrapics (graphics : PIXI.Graphics) (x1,y1) (x2,y2) =
  graphics.moveTo(x1,y1).lineTo(x2,y2)

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
