module AmazingApp

open Browser.Dom
open PixiHal
open PixiHex
open Fable.Pixi.PIXI.Interaction

open Hex
let app = createApplication
document.body.appendChild app.view |> ignore
app.stage.sortableChildren <- true 


let hexAt (parent : Fable.Pixi.PIXI.Container) (sx, sy) = 
  let result = createFlatHexagonGraphics()
  result.x <- sx
  result.y <- sy
  parent.addChild result


let gridGraphics = CreateLineSegmentHexGrid
let HexAtOnGrid = gridGraphics |> hexAt

let hexAtAxial (parent : Fable.Pixi.PIXI.Container) location =
  hexAt parent (location |> axialCoordToPixel)
let hexAtAxialOnGrid = gridGraphics |> hexAtAxial


gridGraphics.x <- 20.
gridGraphics.y <- 20.
gridGraphics.zIndex <- -1.

app.stage.addChild gridGraphics |> ignore

let mouseoverHex =  HexAtOnGrid (0.,0.)

let style = PIXI.TextStyle.Create( stroke = (Fable.Core.U2.Case1 "ffffff")) // gradient

let basicText = PIXI.Text.Create("A grid border", (Fable.Core.U2.Case2 PixiHal.style2))
basicText.x <- 0.
basicText.y <- 0.
basicText |> app.stage.addChild |> ignore

let onMouseMoveHexgrid (grid : Fable.Pixi.PIXI.DisplayObject) (e :InteractionEvent) =
  let point = e.data.``global``
  let localPos = e.data.getLocalPosition grid
  //printfn "%A" (ToCoordinateString (PixiPunkt localPos))
  let mutable text = ToCoordinateString (PixiPunkt point)
  let x = localPos.x
  let y = localPos.y
  Tuple (x,y) |> ToCoordinateString |> fun x -> text <- text + x 

  let ax = Hex.twoDCoordToAxial x y
  let hex2dCoord = axialCoordToPixel ax
  
  Tuple hex2dCoord |> ToCoordinateString |> fun x -> text <- text + x 
  mouseoverHex.x <- fst hex2dCoord
  mouseoverHex.y <- snd hex2dCoord

  text <- text + (sprintf "  %i,%i" ax.q ax.r)

  basicText.text <- text

gridGraphics.interactive <- true
gridGraphics.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointermove, onMouseMoveHexgrid gridGraphics) |> ignore

let location =  axialCoord 13 -1

let positions = 
  flatHexCircleGridAt 3 location
    |> List.map axialCoordToPixel

positions
|> List.map HexAtOnGrid
|> ignore

positions
|> List.map (fun (x,y) -> Tuple (x,y) |> ToCoordinateString) 
|> ignore

let pivot = (TupleToPixiPoint (gridGraphics.width / 2., gridGraphics.height/2.))
gridGraphics.pivot <- pivot
gridGraphics.position <- pivot
printfn "%s" (ToCoordinateString (PixiPunkt pivot))
let update(_) = 
  gridGraphics.rotation <- gridGraphics.rotation + 0.005
  None

app.ticker.add update |> ignore
