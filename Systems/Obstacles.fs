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



module private Systems =
    let load (world: Container) =
            world.On <| fun (LoadContent game) ->
                for bounds in (wallsBound game) do
                    world.Create()
                        .With(Collider.fromRect bounds)
                        .With({WallTexture = colorTexture game Color.Black})
                    |> ignore

    let draw (world: Container) =
        world.On<Draw> <| fun e ->
            for query in world.Query<Wall, Collider>() do
                let struct (wall, collider) = query.Values
                e.SpriteBatch.Draw(wall.WallTexture, collider.ColliderBounds, Color.White)


let configure (world: Container) =
      [ Systems.load world
        Systems.draw world]