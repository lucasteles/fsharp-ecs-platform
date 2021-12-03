module Game.Events

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

[<Struct>] type Start =
            Start of Game
                member _.Data(Start game) = game

[<Struct>] type LoadContent =
            LoadContent of Game
                member _.Data(LoadContent game) = game

[<Struct>] type Update = { DeltaTime: TimeSpan; Game: Game }
[<Struct>] type Draw = { Time: TimeSpan; SpriteBatch: SpriteBatch}