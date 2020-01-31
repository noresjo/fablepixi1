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

let mouseoverHex = hexAt 0. 0. 40.

mouseoverHex.visible <- false
mouseoverHex.ca

let gridGraphics = CreateLineSegmentHexGrid
  
app.stage.addChild gridGraphics |>ignore

gridGraphics.on "mousemove" 

let update(_) = 
  hex.rotation <- hex.rotation + 0.01
  None

app.ticker.add update |> ignore
