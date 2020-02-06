module AmazingApp

open Browser.Dom
open PixiHal
open PixiHex
open Fable.Pixi.PIXI.Interaction
open Hex
let app = createApplication
document.body.appendChild app.view |> ignore
app.stage.sortableChildren <- true 

let hexAt (sx, sy) = 
  let result = createFlatHexagonGraphics()
  result.x <- sx
  result.y <- sy
  result.zIndex <- 100.
  app.stage.addChild result


// [for j in 0 .. 10 -> [ for i in 1..10 -> 
//                                           let x,y = {q=i;r=j}.ToTwoDCoord
//                                           hexAt x y Hex.SCALE]]


let mouseoverHex = hexAt (0.,0.)

mouseoverHex.visible <- true

let gridGraphics = CreateLineSegmentHexGrid

//gridGraphics.x <- 20.
//gridGraphics.y <- 20.
 
app.stage.addChild gridGraphics |> ignore

let style = PIXI.TextStyle.Create( stroke = (Fable.Core.U2.Case1 "ffffff")) // gradient

let basicText = PIXI.Text.Create("A grid border", (Fable.Core.U2.Case2 PixiHal.style2))
basicText.x <- 0.
basicText.y <- 0.
basicText |> app.stage.addChild |> ignore

let onMouseMoveHexgrid (grid : Fable.Pixi.PIXI.DisplayObject) (e :InteractionEvent) =
  let point = e.data.``global``
  let localx = point.x - grid.x
  let localy = point.y - grid.y

  let mutable text = ToCoordinateString (PixiPunkt point)
  let x = localx
  let y = localy
  Tuple (x,y) |> ToCoordinateString |> fun x -> text <- text + x 

  let ax = Hex.twoDCoordToAxial localx localy
  let hex2dCoord = axialCoordToPixel ax
  
  Tuple hex2dCoord |> ToCoordinateString |> fun x -> text <- text + x 
  mouseoverHex.x <- fst hex2dCoord
  mouseoverHex.y <- snd hex2dCoord

  text <- text + (sprintf "  %i,%i" ax.q ax.r)
  
  // text <- text + (sprintf "  %i-%i" ax.cx ax.cy )
  printfn "%A" e.data.target 

  basicText.text <- text

gridGraphics.interactive <- true
gridGraphics.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointermove, onMouseMoveHexgrid gridGraphics) |> ignore

let origo =  axialCoord 13 1 |> axialCoordToPixel
let addOrigo = (addTuple origo)
printfn "%A" (flatHexCircleGrid 1)

let positions = 
  flatHexCircleGrid 3
  |> List.map (axialCoordToPixel >> addOrigo)

positions
|> List.map hexAt
|> ignore

positions
|> List.map (fun (x,y) -> Tuple (x,y) |> ToCoordinateString) 
|> List.map (fun s -> printfn "%s" s)
|> ignore
// let update(_) = 
//   hex.rotation <- hex.rotation + 0.01
//   None

//app.ticker.add update |> ignore
