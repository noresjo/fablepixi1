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

let brickShape = PIXI.Container.Create()
let brickShapeLocations = 
  [Axial.Origo; Axial.Down; Axial.Up; Axial.DownLeft; Axial. UpRight ]

brickShapeLocations
|> List.map (hexAtAxial brickShape) |> ignore

let createHexBrick = brickShape

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
  
  snapTo brickShape (fst pixelCoords) (snd pixelCoords) |> ignore
  
  // brickShape.x <- fst pixelCoords
  // brickShape.y <- snd pixelCoords
  
  statusText.text <- text

let onMouseUpBoard (board : Fable.Pixi.PIXI.DisplayObject) (e :InteractionEvent) =
  let localPos = e.data.getLocalPosition board
  let offset = Hex.twoDCoordToAxial localPos.x localPos.y  
  let addAxialOffset = axialAdd offset
  let addAtMouse = (addAxialOffset >> hexAtAxialOnBoard )
  
  brickShapeLocations 
  |> List.map addAtMouse
  |> ignore


board.interactive <- true
board.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointermove, onMouseMove board) |> ignore
board.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointerup, onMouseUpBoard board) |> ignore

