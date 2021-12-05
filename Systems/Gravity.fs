module Game.Systems.Gravity

open Game.Components
open Game.Events
open Garnet.Composition

let private applyG (Velocity vel) (Gravity g) =
    vel.WithY(vel.Y + g) |> Velocity

let configure (world: Container) = [
    world.On<Update> <|
        fun _ ->
            for query in world.Query<Velocity, Gravity>() do
                let gravity = query.Value2
                let velocity = &query.Value1
                velocity <- applyG velocity gravity
    ]

