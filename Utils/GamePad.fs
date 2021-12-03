module GamePad

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open VectorModule

let (|ButtonDown|_|) k (state: GamePadState) =
    if state.IsButtonDown k then Some() else None

let movementVector =
    function
    | ButtonDown Buttons.DPadUp & ButtonDown Buttons.DPadLeft -> Vector2(-1.f, -1.f)
    | ButtonDown Buttons.DPadUp & ButtonDown Buttons.DPadRight -> Vector2(1.f, -1.f)
    | ButtonDown Buttons.DPadDown & ButtonDown Buttons.DPadLeft -> Vector2(-1.f, 1.f)
    | ButtonDown Buttons.DPadDown & ButtonDown Buttons.DPadRight -> Vector2(1.f, 1.f)
    | ButtonDown Buttons.DPadUp -> Vector2(0.f, -1.f)
    | ButtonDown Buttons.DPadDown -> Vector2(0.f, 1.f)
    | ButtonDown Buttons.DPadLeft -> Vector2(-1.f, 0.f)
    | ButtonDown Buttons.DPadRight -> Vector2(1.f, -0.f)
    | _ as k -> k.ThumbSticks.Left * (vector2 1 -1)
