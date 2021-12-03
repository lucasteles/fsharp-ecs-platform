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

    let updateRotation deltaTime  rot =
        rot + 0.1f * deltaTime |> Rotation

    let updateScale deltaTime scale =
        if (scale < 2f)
        then scale + 2f * deltaTime
        else scale
        |> Scale

    let updatePosition deltaTime position velocity =
        Position (position + velocity * deltaTime)

    let drawLogo (spriteBatch: SpriteBatch)
                 (logo: Player)
                 (Position position)
                 (Rotation rotation)
                 (Scale scale) =
        let logoCenter =
            (vector2
                logo.Texture.Bounds.Width
                logo.Texture.Bounds.Height) / 2f

        spriteBatch.Draw(
            logo.Texture,
            position,
            logo.Texture.Bounds,
            Color(255, 255, 255, 80),
            rotation,
            logoCenter,
            scale,
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
                    entity.Add(Logic.startPosition game)
                    entity.Add(Rotation 0f)
                    entity.Add(Scale 0f)
                    entity.Add(PlayerInput.zero)
                    entity.Add(Vector2.Zero |> Velocity)

    let scaleLogo (world: Container) =
            world.On<Update> <| fun update ->
                for query in world.Query<Scale, Player>() do
                    let (Scale value) = query.Value1
                    let comp = &query.Value1
                    comp <- Logic.updateScale update.DeltaTime.seconds value


    let rotateLogo (world: Container) =
            world.On<Update> <| fun update ->
                for query in world.Query<Rotation, Player>() do
                    let (Rotation rot) = query.Value1
                    let comp = &query.Value1
                    comp <- Logic.updateRotation update.DeltaTime.seconds rot

    let updateVelocity (world: Container) =
            world.On<Update> <| fun _ ->
                for query in world.Query<Velocity, PlayerInput, Player>() do
                   let struct (_,input,player) = query.Values
                   let comp = &query.Value1
                   comp <- (input.Direction * player.Speed) |> Velocity

    let updateLogoPosition (world: Container) =
            world.On<Update> <| fun update ->
                for query in world.Query<Position, Velocity, Player>() do
                    let struct (Position position, Velocity velocity, _) = query.Values
                    let comp = &query.Value1
                    comp <- Logic.updatePosition update.DeltaTime.seconds position velocity

    let drawLogo (world: Container) =
        world.On<Draw> <| fun e ->
            for query in world.Query<Rotation, Scale, Position, Player>() do
                let struct (rot, scale, pos, player) = query.Values
                Logic.drawLogo e.SpriteBatch player pos rot scale

let configure (world: Container) =
    [
       Systems.load world
       Systems.start world
       Systems.scaleLogo world
       Systems.rotateLogo world
       Systems.updateVelocity world
       Systems.updateLogoPosition world
       Systems.drawLogo world
    ]