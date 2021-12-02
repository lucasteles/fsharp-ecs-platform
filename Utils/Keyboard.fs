module Keyboard

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let (|KeyDown|_|) k (state: KeyboardState) =
    if state.IsKeyDown k then Some() else None

let arrowLayout = Keys.Left,Keys.Down,Keys.Up,Keys.Right
let wsadLayout = Keys.A,Keys.S,Keys.W,Keys.D
let vimLayout = Keys.H,Keys.J,Keys.K,Keys.L

let movementVectorFor (left, down, up, right) =
    function
    | KeyDown up & KeyDown left -> Vector2(-1.f, -1.f)
    | KeyDown up & KeyDown right -> Vector2(1.f, -1.f)
    | KeyDown down & KeyDown left -> Vector2(-1.f, 1.f)
    | KeyDown down & KeyDown right -> Vector2(1.f, 1.f)
    | KeyDown up -> Vector2(0.f, -1.f)
    | KeyDown down -> Vector2(0.f, 1.f)
    | KeyDown left -> Vector2(-1.f, 0.f)
    | KeyDown right -> Vector2(1.f, -0.f)
    | _ -> Vector2.Zero

let layoutsToUse = [|arrowLayout; wsadLayout; vimLayout|]

let movementVector keyboardState =
    layoutsToUse
    |> Array.sumBy (fun l -> movementVectorFor l keyboardState)
    |> divideBy (single layoutsToUse.Length)