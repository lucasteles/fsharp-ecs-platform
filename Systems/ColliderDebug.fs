module Game.Systems.ColliderDebug

open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open MonoGame.Extended

module private Systems =
    let draw (world: Container) =
            world.On<Draw> <| fun e ->
                for actor in world.Query<Collider, Eid>() do
                    let struct (collider, eid) = actor.Values
                    let entity = world.Get eid
                    match entity.TryGet<Transform>() with
                    | true, {Position=Position pos} ->
                        e.SpriteBatch.DrawRectangle(collider.ColliderBounds.OffsetValue(pos).ToRectangleF(), Color.Yellow, 5f)
                    | false, _ ->
                        e.SpriteBatch.DrawRectangle(collider.ColliderBounds.ToRectangleF(), Color.Green, 5f)


let configure (world: Container) =
      [ //Systems.drawStatic world
        Systems.draw world ]