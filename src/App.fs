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


let gridGraphics = CreateLineSegmentHexGrid
  
app.stage.addChild gridGraphics |>ignore

// Hex.flatHexGrid 10 5 40
// |> List.map (fun (x,y) -> hexAt x y 40.)
// |> ignore

// let update(_) = 
//   hex.rotation <- hex.rotation + 0.01
//   None

// app.ticker.add update |> ignore
