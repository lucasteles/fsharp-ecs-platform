module Game.Systems.Input

open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open Game.Components.Features

let direction () =
    let keyboard = Keyboard.GetState()
    let gamepad = GamePad.GetState(PlayerIndex.One)
    let kbDir = Keyboard.movementVector keyboard
    let padDir = GamePad.movementVector gamepad
    let dir = kbDir + padDir
    Vector2.normalize dir

let configure (world: Container) = [
    // quit game system
    world.On<Update>(
        fun _ (_: PlayerInput) ->
            { Direction = direction() }
        |> Join.update1
        |> Join.over world)
    ]
