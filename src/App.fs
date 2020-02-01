module AmazingApp

open Browser.Dom
open PixiHal

let app = createApplication
document.body.appendChild app.view |> ignore

let hexAt sx sy size = 
  let result = createFlatHexagonGraphics size
  result.x <- sx
  result.y <- sy
  app.stage.addChild result

let mouseoverHex = hexAt 0. 0. Hex.SCALE

mouseoverHex.visible <- false

let gridGraphics = CreateLineSegmentHexGrid
  
app.stage.addChild gridGraphics |>ignore

let style = PIXI.TextStyle.Create( stroke = (Fable.Core.U2.Case1 "ffffff")) // gradient

let basicText = PIXI.Text.Create("hello", (Fable.Core.U2.Case2 PixiHal.style2))|> app.stage.addChild
basicText.x <- 50.
basicText.y <- 100.


let onMouseMoveHexgrid = 
  ignore //text. <- "hsdf"

gridGraphics.on( "mousemove", fun () -> onMouseMoveHexgrid()) |> ignore

// let update(_) = 
//   hex.rotation <- hex.rotation + 0.01
//   None

//app.ticker.add update |> ignore
