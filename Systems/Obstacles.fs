module Game.Systems.Obstacles

open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework


let wallSize = 100
let wallsBound (game: Game) = [
    Rectangle(0,0,game.Window.ClientBounds.Width,wallSize)
    Rectangle(0,game.Window.ClientBounds.Height-wallSize,game.Window.ClientBounds.Width,wallSize)
]

let load (world: Container) =
        world.On <| fun (LoadContent game) ->
            for bounds in (wallsBound game) do
                let collider = Collider.fromRect bounds
                let texture = colorTexture game Color.Black
                world.Create()
                    .With(collider)
                    .With(Wall)
                    .With(SpriteRenderer.create (texture, collider.ColliderBounds))
                |> ignore



let configure (world: Container) =
      [ load world ]