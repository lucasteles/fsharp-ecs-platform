module Game.Systems.Input

open Game.Components
open Game.Events
open Keyboard
open GamePad
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let direction kbState padState =
    let kbDir = kbState |> Keyboard.movementVector
    let padDir = padState |> GamePad.movementVector
    let dir = kbDir + padDir
    Vector2.normalize dir

let jump kbState padState =
    let kbJump =
        match kbState with
        | KeyDown Keys.Space -> true
        | _ -> false
    let padJump =
        match padState with
        | ButtonDown Buttons.A -> true
        | _ -> false
    if kbJump || padJump
    then PlayerButtomState.Pressed
    else PlayerButtomState.Released

let configure (world: Container) = [
    world.On<Update> <|
        fun _ ->
            let kbState = Keyboard.GetState()
            let padState = GamePad.GetState(PlayerIndex.One)
            for query in world.Query<PlayerInput>() do
                let playerInput = &query.Value
                playerInput <- { Direction = direction kbState padState
                                 Jump = jump kbState padState }
    ]

