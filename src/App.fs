module AmazingApp

open Browser.Dom
open PixiHal
open PixiHex
open Fable.Pixi.PIXI.Interaction

open Hex
let app = createApplication
document.body.appendChild app.view |> ignore
app.stage.sortableChildren <- true 

let board = CreateHexBoard |> CreateBordBorder
let HexAtOnBoard = board |> hexAt
let hexAtAxialOnBoard = board |> hexAtAxial

board.x <- 20.
board.y <- 40.
board.zIndex <- -1.

app.stage.addChild board |> ignore

let basicText = PIXI.Text.Create("statustext", (Fable.Core.U2.Case2 PixiHal.style2))
basicText.x <- 0.
basicText.y <- 0.
basicText |> app.stage.addChild |> ignore

let onMouseMoveHexgrid (grid : Fable.Pixi.PIXI.DisplayObject) (e :InteractionEvent) =
  let localPos = e.data.getLocalPosition grid
  //printfn "%A" (ToCoordinateString (PixiPunkt localPos))
  let mutable text = "Mousemove "
  let x = localPos.x
  let y = localPos.y  
  Tuple (x,y) |> ToCoordinateString |> fun x -> text <- text + x 

  let ax = Hex.twoDCoordToAxial x y
  
  text <- text + (sprintf "  %i,%i" ax.q ax.r)

  basicText.text <- text

board.interactive <- true
board.on( InteractionEventTypes.ofInteractionPointerEvents InteractionPointerEvents.Pointermove, onMouseMoveHexgrid board) |> ignore

// let update(_) = 
//   board.rotation <- board.rotation + 0.0005
//   None

// app.ticker.add update |> ignore
