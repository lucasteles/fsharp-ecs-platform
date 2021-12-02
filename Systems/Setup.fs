module GameLogic

open Game.Systems
open Garnet.Composition

let configureWorld (world: Container) =

    let configs = [
        Input.configure world
        Player.configure world
        Exit.configure world
    ]

    configs |> List.collect id
