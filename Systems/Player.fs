module Player

open Game.Components.Features
open Game.Components.Transform
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let create (game: Game) = {
    Texture = game.Content.Load("logo")
    Speed = 150f
}

module private Logic =
    let startPosition (game: Game) =
        let clientBounds = game.Window.ClientBounds
        Position.create
            (single clientBounds.Width / 2f)
            (single clientBounds.Height / 2f)

    let updateRotation deltaTime  (Rotation rot) =
        rot + 0.1f<Radians> * deltaTime |> Rotation

    let updateScale deltaTime (ScalarScale scale) =
        if (scale < 2f)
        then scale + 2f * deltaTime
        else scale
        |> ScalarScale

    let updatePosition deltaTime (Position position) velocity =
        Position (position + velocity * deltaTime)

    let drawLogo (spriteBatch: SpriteBatch) player transform =

        let logoCenter =
            (vector2
                player.Texture.Bounds.Width
                player.Texture.Bounds.Height) / 2f

        spriteBatch.Draw(
            player.Texture,
            transform,
            player.Texture.Bounds,
            Color(255, 255, 255, 80),
            logoCenter,
            SpriteEffects.None,
            0f
        )

module private Systems =
    let load (world: Container) =
            world.On <| fun (LoadContent game) ->
                world.Create()
                     .With(create game) |> ignore

    let start (world: Container) =
            world.On <| fun (Start game)  ->
                for query in world.Query<Eid, Player>() do
                    let entity = world.Get query.Value1
                    Transform.create (Logic.startPosition game, Rotation 0f<Radians>, ScalarScale 0f) |> entity.Add
                    PlayerInput.zero |> entity.Add
                    Vector2.Zero |> Velocity |> entity.Add

    let animatePlayer (world: Container) =
            world.On<Update> <| fun update ->
                for query in world.Query<Transform, Player>() do
                    let transform = query.Value1
                    let comp = &query.Value1
                    comp <- { transform with
                                  ScalarScale = Logic.updateScale update.DeltaTime.seconds transform.ScalarScale
                                  Rotation = Logic.updateRotation update.DeltaTime.seconds transform.Rotation }

    let updatePlayerPosition (world: Container) =
            world.On<Update> <| fun update ->
                for query in world.Query<Transform, Velocity, Player>() do
                    let struct ({Position = position} as transform,
                                Velocity velocity, _) = query.Values
                    let comp = &query.Value1
                    comp <- { transform with Position = Logic.updatePosition update.DeltaTime.seconds position velocity }

    let updateVelocity (world: Container) =
            world.On<Update> <| fun _ ->
                for query in world.Query<Velocity, PlayerInput, Player>() do
                   let struct (_,input,player) = query.Values
                   let comp = &query.Value1
                   comp <- (input.Direction * player.Speed) |> Velocity

    let drawLogo (world: Container) =
        world.On<Draw> <| fun e ->
            for query in world.Query<Transform, Player>() do
                let struct (transform, player) = query.Values
                Logic.drawLogo e.SpriteBatch player transform

let configure (world: Container) =
    [
       Systems.load world
       Systems.start world
       Systems.animatePlayer world
       Systems.updateVelocity world
       Systems.updatePlayerPosition world
       Systems.drawLogo world
    ]