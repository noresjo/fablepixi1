module AmazingApp

open Browser.Dom
open PixiHal
open PixiHex
open Fable.Pixi.PIXI.Interaction

open Hex
let app = createApplication
document.body.appendChild app.view |> ignore
app.stage.sortableChildren <- true 

let board = CreateHexBoard |> CreateBoardBorder
let HexAtOnBoard = board |> hexAt
let hexAtAxialOnBoard = board |> hexAtAxial

board.x <- 20.
board.y <- 40.
board.zIndex <- -1.

app.stage.addChild board |> ignore

let statusText = PIXI.Text.Create("statustext", (Fable.Core.U2.Case2 PixiHal.style2))
statusText.x <- 0.
statusText.y <- 0.
statusText |> app.stage.addChild |> ignore

let brickShapeLocations = 
  [Axial.Origo; Axial.Down; Axial.Up; Axial.DownLeft; Axial. UpRight ]

let mutable brickShape = PIXI.Graphics.Create()
let hexesInShape = brickShapeLocations |> List.map (hexAtAxial brickShape) 
//brickShape.cacheAsBitmap <- true
brickShape |> board.addChild |> ignore


let onMouseMove (board : Fable.Pixi.PIXI.DisplayObject) (e :InteractionEvent) =
  let localPos = e.data.getLocalPosition board
  //printfn "%A" (ToCoordinateString (PixiPunkt localPos))
  let mutable text = "Mousemove "
  let x = localPos.x
  let y = localPos.y  
  Tuple (x,y) |> ToCoordinateString |> fun x -> text <- text + x 

  let ax = Hex.twoDCoordToAxial x y
  let pixelCoords = ax |> axialCoordToPixel
  text <- text + (sprintf "  %i,%i" ax.q ax.r)
  
  if not (List.forall onBoard (brickShapeLocations |> List.map (axialAdd ax))) then
    printfn "%i" brickShape.children.Count
    hexesInShape |> List.map (fun x -> x.tint <- float 0xff0000) |> ignore
    printfn "%i" hexesInShape.Length
  else
    hexesInShape |> List.map (fun x -> x.tint <- float 0xffffff) |> ignore
  
 

  snapTo brickShape (fst pixelCoords) (snd pixelCoords) |> ignore
  statusText.text <- text

let onMouseUpBoard (board : Fable.Pixi.PIXI.DisplayObject) (e :InteractionEvent) =
  let localPos = e.data.getLocalPosition board
  let offset = Hex.twoDCoordToAxial localPos.x localPos.y  
  let addAxialOffset = axialAdd offset
  
  brickShapeLocations 
  |> List.map addAxialOffset
  |> List.filter onBoard 
  |> List.map hexAtAxialOnBoard
  |> ignore


board.interactive <- true
board.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointermove, onMouseMove board) |> ignore
board.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointerup, onMouseUpBoard board) |> ignore

