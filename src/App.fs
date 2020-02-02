module AmazingApp

open Browser.Dom
open PixiHal
open Fable.Pixi.PIXI.Interaction
open Fable.Pixi.PIXI

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

gridGraphics.x <- 20.
gridGraphics.y <- 20.
 
app.stage.addChild gridGraphics |> ignore

let style = PIXI.TextStyle.Create( stroke = (Fable.Core.U2.Case1 "ffffff")) // gradient

let basicText = PIXI.Text.Create("A grid border", (Fable.Core.U2.Case2 PixiHal.style2))
basicText.x <- 0.
basicText.y <- 0.
basicText |> app.stage.addChild |> ignore

type Coordinate =
  | PixiPunkt of Fable.Pixi.PIXI.Point
  | Tuple of float * float

let ToCoordinateString (c : Coordinate) =
  let result x y = 
    sprintf "(%i,%i)" (x |> int) (y |> int)

  match c with
  | PixiPunkt c -> result c.x c.y
  | Tuple (x,y) -> result x y


let onMouseMoveHexgrid (grid : Fable.Pixi.PIXI.DisplayObject) (e :InteractionEvent) =
  let point = e.data.``global``
  let localx = point.x - grid.x
  let localy = point.y - grid.y

  let mutable text = ToCoordinateString (PixiPunkt point)
  let x = localx
  let y = localy
  Tuple (x,y) |> ToCoordinateString |> fun x -> text <- text + x 

  let hv = Hex.pointToHexVector x y
  text <- text + (sprintf "  %i-%i-%i %b" hv.HX hv.HY hv.HZ hv.isValid)
  printfn "%A" e.data.target 

  basicText.text <- text

gridGraphics.interactive <- true
gridGraphics.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointermove, onMouseMoveHexgrid gridGraphics) |> ignore

// let update(_) = 
//   hex.rotation <- hex.rotation + 0.01
//   None

//app.ticker.add update |> ignore
