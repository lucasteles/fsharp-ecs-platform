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
            world.On (fun (LoadContent game) ->
                        world.Create().With(create game) |> ignore)

    let start (world: Container) =
            world.On(
                fun (Start game) struct (eid: Eid, _: Player) ->
                    let entity = world.Get eid
                    entity.Add(Logic.startPosition game)
                    entity.Add(Rotation 0f)
                    entity.Add(Scale 0f)
                    entity.Add(PlayerInput.zero)
                    entity.Add(Vector2.Zero |> Velocity)
                    eid
                |> Join.update2
                |> Join.over world
            )


    let scaleLogo (world: Container) =
            world.On<Update>(
                fun update struct (Scale scale, _: Player) ->
                    Logic.updateScale update.DeltaTime.seconds scale
                |> Join.update2
                |> Join.over world
            )

    let rotateLogo (world: Container) =
            world.On<Update>(
                fun update struct (Rotation rot, _: Player) ->
                    Logic.updateRotation update.DeltaTime.seconds rot
                |> Join.update2
                |> Join.over world
            )


    let updateVelocity (world: Container) =
            world.On<Update>(
                fun _ struct (Velocity _, input: PlayerInput, player: Player) ->
                    Velocity (input.Direction * player.Speed)
                |> Join.update3
                |> Join.over world
            )
    let updateLogoPosition (world: Container) =
            world.On<Update>(
                fun update struct (Position position, Velocity velocity, _: Player) ->
                    Logic.updatePosition update.DeltaTime.seconds
                                   position
                                   velocity
                |> Join.update3
                |> Join.over world
            )
    let drawLogo (world: Container) =
        world.On<Draw>(
            fun e struct (rot: Rotation, scale: Scale, pos: Position, player: Player) ->
                Logic.drawLogo e.SpriteBatch player pos rot scale
            |> Join.iter4
            |> Join.over world
        )

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