module AmazingApp

open Browser.Dom
open PixiHal

let app = createApplication
document.body.appendChild app.view |> ignore

let hex = 
  let result = createFlatHexagonGraphics 30.
  result.x <- 30.
  result.y <- 30.
  result
  
app.stage.addChild hex|> ignore

let update(_) = 
  hex.rotation <- hex.rotation + 0.01
  None

app.ticker.add update |> ignore
