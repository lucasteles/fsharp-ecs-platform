module Game.Events

open System
open Game.Scenes
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

[<Struct>] type Start =
            Start of Game
                member _.Data(Start game) = game
[<Struct>] type Stop = Stop of Game
[<Struct>] type UnloadContent = UnloadContent of Game
[<Struct>] type LoadContent =
            LoadContent of Game
                member _.Data(LoadContent game) = game

[<Struct>]
type TriggerEnter =
    { Game: Game; TriggerFrom:Eid; FromBounds:Rectangle; Other: Eid; Bounds: Rectangle; Overlap: Rectangle }
    member this.Is(eid: Eid) = this.TriggerFrom = eid

[<Struct>]
type CollisionEnter =
    { Game: Game; CollideFrom:Eid; FromBounds:Rectangle; Other: Eid; Bounds: Rectangle; Overlap: Rectangle }
    member this.Is(eid: Eid) = this.CollideFrom = eid

[<Struct>] type Update = { DeltaTime: TimeSpan; Game: Game; ChangeScene: SceneName -> unit }
[<Struct>] type Draw = { DeltaTime: TimeSpan; SpriteBatch: SpriteBatch; Game: Game}
