module Game.Systems.Input

open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let direction () =
    let kbDir = Keyboard.GetState() |> Keyboard.movementVector
    let padDir = GamePad.GetState(PlayerIndex.One) |> GamePad.movementVector
    let dir = kbDir + padDir
    Vector2.normalize dir

let configure (world: Container) = [
    world.On<Update> <|
        fun _ ->
            for query in world.Query<PlayerInput>() do
                let playerInput = &query.Value
                playerInput <- { Direction = direction() }
    ]

