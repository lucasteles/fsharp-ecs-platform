module Player

open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


[<Literal>]
let floorTriggerName = "FloorTrigger"

module private Logic =

    let create (game: Game) = {
        Speed = 300f
        Size = Vector2(50f,80f)
        JumpForce = 1100f
        PlayerState = PlayerState.Idle
    }

    let startPosition (game: Game) =
        let clientBounds = game.Window.ClientBounds
        Position.create
            (single clientBounds.Width / 4f)
            (single clientBounds.Height / 2f)

    let updatePosition deltaTime (Position position) velocity =
        Position (position + velocity * deltaTime)

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
                    entity.Add (Transform.create(Logic.startPosition game, Rotation 0f<Radians>, Scale Vector2.One))
                    entity.Add PlayerInput.zero
                    entity.Add (Velocity Vector2.Zero)
                    entity.Add Gravity.default'
                    entity.Add (Collider.group [|
                                  (Collider.create (-player.Size/2f) player.Size)
                                  (Collider.trigger floorTriggerName
                                                    (vector2 (-player.Size.X/2f + 5f) (player.Size.Y/2f))
                                                    (vector2 (player.Size.X-10f) 5f))
                              |])
                    entity.Add (SpriteRenderer.create (colorTexture game Color.DarkRed,
                                                       rect Vector2.Zero player.Size,
                                                       Color.White,
                                                       player.Size / 2f,
                                                       SpriteEffects.None, 0f) )

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

                   if jump then
                       let playerRef = &query.Value3
                       playerRef  <- { player with PlayerState = PlayerState.Jump }

                   let velocityRef = &query.Value1
                   velocityRef <- Velocity.create (input.Direction.X * player.Speed)
                                                  (vel.Y + (if jump then -player.JumpForce else 0f))

    let trigger (world: Container) =
        world.On<TriggerEnter> <| fun trigger ->
            if trigger.TriggerFrom.Name = floorTriggerName then
                for query in world.Query<Player, PlayerInput, Eid>() do
                    if trigger.Is(query.Value3) then
                        let player = query.Value1
                        let playerRef = &query.Value1
                        if player.PlayerState = PlayerState.Jump then
                            playerRef <- {player with PlayerState = PlayerState.Idle}

let configure (world: Container) =
    [
       Systems.load world
       Systems.start world
       Systems.updateVelocity world
       Systems.updatePlayerPosition world
       Systems.trigger world
    ]