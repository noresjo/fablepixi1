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
      x.antialias <- Some(false)
      x.backgroundColor <-Some(Constants.Background)
      )

  PIXI.Application.Create(options)

let style2 = jsOptions< PIXI.TextStyle>(fun x ->
      x.fill <- Fable.Core.U6.Case3 Constants.TextColor
      x.fontSize <- Fable.Core.U2.Case1 12.
  )

let centerPivot (displayObject : PIXI.DisplayObject) =
  let bounds = displayObject.getBounds()
  displayObject.pivot <- PIXI.Point.Create(
    bounds.x/2.,
    bounds.y/2.)



open Fable.Core.JsInterop


// let inline sendToJs (opts: PixiEase.PIXI.AddOption list) =
// keyValueList Fable.Core.CaseRules.LowerFirst opts
    
// let test : PixiEase.PIXI.EaseParams = !!{| x = 60; y = 40 |}
// let test2 : PixiEase.PIXI.AddOptions = !!{|duration = 1000 |}
// ease.add(square, { x: 20 }, { duration: 200, ease: 'easeInOutSine' })

let snapEaseAddOptions = 
  jsOptions<PixiEase.PIXI.AddOptions> (fun x ->
//    x.reverse <- Some true
    x.duration <- Some 100.
//    x.repeat <- Some (Fable.Core.U2.Case1 true)
    x.ease <- Some "easeOutSine" )

let snapEaseParams x y = 
  jsOptions<PixiEase.PIXI.EaseParams> (fun a ->
//    x.reverse <- Some true
    a.x <- Some x
//    x.repeat <- Some (Fable.Core.U2.Case1 true)
    a.y <- Some y )

let snapTo displayObject x y =
  PixiEase.PIXI.ease.add(displayObject, (snapEaseParams x y), snapEaseAddOptions) |> ignore
  displayObject

// PixiEase.PIXI.ease.add(board,
//   !!{| 
//     x = 100.
//     y = 40. 
//   |},
// |> ignore

// let foo = jsOptions<PixiEase.PIXI.AddOptions>


// let update(_) = 
//   board.rotation <- board.rotation + 0.0005
//   None

// app.ticker.add update |> ignore
