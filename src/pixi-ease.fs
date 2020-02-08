// ts2fable 0.0.0
module rec PixiEase
open Fable.Core
open Fable.Pixi

module PIXI =
type EventEmitter = Fable.Pixi.PIXI.Utils.EventEmitter
let [<Import("ease", from="pixi-ease")>] ease: Ease = jsNative
// import { Ease, ease } from 'pixi-ease'

// [<Import("myFunction",, from="my-module")>]
// import { myFunction } from "my-module"

type [<AllowNullLiteral>] IExports =
    abstract Ease: EaseStatic
    abstract EaseDisplayObject: EaseDisplayObjectStatic

type [<AllowNullLiteral>] EaseOptions =
    abstract duration: float option with get, set
    abstract ease: string option with get, set
    abstract useTicker: bool option with get, set
    abstract ticker: PIXI.Ticker option with get, set
    abstract maxFrame: float option with get, set

type [<AllowNullLiteral>] EaseParams =
    abstract x: float option with get, set
    abstract y: float option with get, set
    abstract position: U2<PIXI.DisplayObject, PIXI.Point> option with get, set
    abstract width: float option with get, set
    abstract height: float option with get, set
    abstract scale: float option with get, set
    abstract scaleX: float option with get, set
    abstract scaleY: float option with get, set
    abstract alpha: float option with get, set
    abstract rotation: float option with get, set
    abstract face: U2<PIXI.DisplayObject, PIXI.Point> option with get, set
    abstract skew: float option with get, set
    abstract skewX: float option with get, set
    abstract skewY: float option with get, set
    abstract tint: U2<float, ResizeArray<float>> option with get, set
    abstract blend: U2<float, ResizeArray<float>> option with get, set
    abstract shake: float option with get, set
   // [<Emit "$0[$1]{{=$2}}">] abstract Item: generic: string -> obj option with get, set

type [<AllowNullLiteral>] AddOptions =
    abstract duration: float option with get, set
    abstract ease: string option with get, set
    abstract repeat: U2<bool, float> option with get, set
    abstract reverse: bool option with get, set
    abstract wait: float option with get, set
    abstract removeExisting: bool option with get, set

type [<AllowNullLiteral>] Ease =
    inherit EventEmitter
    abstract duration: float with get, set
    abstract ease: string with get, set
    abstract destroy: unit -> unit
    abstract add: element: PIXI.DisplayObject * ``params``: EaseParams * options: AddOptions -> U2<EaseDisplayObject, ResizeArray<EaseDisplayObject>>
    abstract removeAllEases: element: PIXI.DisplayObject -> unit
    abstract removeEase: element: PIXI.DisplayObject * param: U2<string, ResizeArray<string>> -> unit
    abstract removeAll: force: bool -> unit
    abstract update: unit -> unit
    abstract countElements: unit -> float
    abstract countRunning: unit -> float

type [<AllowNullLiteral>] EaseStatic =
    [<Emit "new $0($1...)">] abstract Create: options: EaseOptions -> Ease

type [<AllowNullLiteral>] EaseDisplayObject =
    inherit EventEmitter
    abstract count: float with get, set
    abstract remove: ``params``: U2<string, ResizeArray<string>> -> unit
    abstract add: ``params``: EaseParams * options: AddOptions -> unit
    abstract update: elapsed: float -> unit

type [<AllowNullLiteral>] EaseDisplayObjectStatic =
    [<Emit "new $0($1...)">] abstract Create: element: PIXI.DisplayObject * ease: Ease -> EaseDisplayObject
