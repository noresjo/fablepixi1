module AmazingApp

open Browser.Dom
open System
open Fable.Pixi
let PIXI = PIXI.pixi
open Fable.Core.JsInterop

let PIXI = Fable.Pixi.PIXI.pixi

let options  = jsOptions<Fable.Pixi.PIXI.ApplicationStaticOptions>(fun x ->
    x.antialias <- Some(true)
    )

let app = PIXI.Application.Create(options)
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
let graphics = PIXI.Graphics.Create().beginFill((float)0xDE3249).drawRect(0., 0., 100., 100.).endFill()
let graphics2 = PIXI.Graphics.Create().lineStyle(4.0, (float)0xFFBD01, 1.).beginFill((float)0xC34288).drawRect(0., 0., 100., 100.).endFill();

graphics.x <- 100.
graphics2.x <- 400.
graphics.y <- 350.
graphics2.y <- 200.


let gfx = [
    graphics
    graphics2
]

gfx 
|> List.map ((fun (x) -> x.pivot.x <- 50.;x.pivot.y <- 50.; x ) >> app.stage.addChild) 
|> ignore

let update(_) = 
    gfx 
    |> List.mapi (fun i (x : Fable.Pixi.PIXI.Graphics) -> x.angle <- x.angle + ((float)i+1.)*1.) |> ignore
    graphics2.y <- System.Math.Sin(graphics2.rotation) * 100. + 200.
    None

app.ticker.add update |> ignore
