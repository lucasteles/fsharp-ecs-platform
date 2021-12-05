﻿module Player

open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open VectorModule
open VectorModule
open VectorModule
open VectorModule
open VectorModule

module private Logic =

    let create (game: Game) = {
        Texture = colorTexture game Color.DarkRed
        Speed = 300f
        Size = Vector2(50f,80f)
        JumpForce = 1000f
        PlayerState = PlayerState.Idle
    }

    let startPosition (game: Game) =
        let clientBounds = game.Window.ClientBounds
        Position.create
            (single clientBounds.Width / 2f)
            (single clientBounds.Height / 2f)

    let updatePosition deltaTime (Position position) velocity =
        Position (position + velocity * deltaTime)

    let drawLogo (spriteBatch: SpriteBatch) player transform =

        spriteBatch.Draw(
            player.Texture,
            transform,
            rect Vector2.Zero player.Size,
            Color.White,
            player.Size / 2f,
            SpriteEffects.None,
            0f
        )

module private Systems =
    let load (world: Container) =
            world.On <| fun (LoadContent game) ->
                world.Create()
                     .With(Logic.create game) |> ignore

    let start (world: Container) =
            world.On <| fun (Start game)  ->
                for query in world.Query<Eid, Player>() do
                    let player = query.Value2
                    let entity = world.Get query.Value1
                    let transform = (Logic.startPosition game, Rotation 0f<Radians>, Scale Vector2.One) |> Transform.create
                    transform |> entity.Add
                    PlayerInput.zero |> entity.Add
                    Vector2.Zero |> Velocity |> entity.Add
                    Gravity.default' |> entity.Add
                    {ColliderBounds = rect (-player.Size/2f) player.Size} |> entity.Add

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
                   let struct (Velocity vel,input,player) = query.Values
                   let jump = input.Jump = PlayerButtomState.Pressed && player.PlayerState <> PlayerState.Jump

                   let velocityRef = &query.Value1
                   velocityRef <- Vector2(
                               input.Direction.X * player.Speed,
                               vel.Y + (if jump then -player.JumpForce else 0f)
                           ) |> Velocity
                   if jump then
                       let playerRef = &query.Value3
                       playerRef  <- { player with PlayerState = PlayerState.Jump }


    let collision (world: Container) =
        world.On<CollisionEnter> <| fun info ->
            for query in world.Query<Player, Eid>() do
                if info.From = query.Value2 then
                    let player = query.Value1
                    let playerRef = &query.Value1
                    if player.PlayerState = PlayerState.Jump then
                        playerRef <- {player with PlayerState = PlayerState.Idle}


    let draw (world: Container) =
        world.On<Draw> <| fun e ->
            for query in world.Query<Transform, Player>() do
                let struct (transform, player) = query.Values
                Logic.drawLogo e.SpriteBatch player transform

let configure (world: Container) =
    [
       Systems.load world
       Systems.start world
       Systems.updateVelocity world
       Systems.updatePlayerPosition world
       Systems.draw world
       Systems.collision world
    ]