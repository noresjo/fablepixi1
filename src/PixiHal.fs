module PixiHal

open Hex
open PixiEase
open Fable.Pixi
open Fable.Core.JsInterop

let PIXI = Fable.Pixi.PIXI.pixi

let Constants = {|
  Background = (float 0x193549)
  BoardBorderColor = (float 0x04080a)
  BoardBackground = (float 0x0050A4)
  GridAlpha = 1.
  TextColor = (float 0xc6ff00)
  HexagonLineColor = (float 0x04080a)
  HexagonFillColor = (float 0x93c247)
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
      x.fill <- !^ Constants.TextColor
      x.fontSize <- !^ 12.
  )

let centerPivot (displayObject : PIXI.DisplayObject) =
  let bounds = displayObject.getBounds()
  displayObject.pivot <- PIXI.Point.Create(
    bounds.x/2.,
    bounds.y/2.)

let snapEaseAddOptions =
  [
    (Duration 500.)
    (Ease "easeOutCirc")
  ]

let snapEaseParams x y = 
  [
    X x
    Y y
  ]

let snapTo displayObject x y =
  ease displayObject (snapEaseParams x y) (snapEaseAddOptions)
