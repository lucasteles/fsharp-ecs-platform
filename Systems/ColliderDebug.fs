module Game.Systems.ColliderDebug

open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open MonoGame.Extended

module private Systems =
    let draw (world: Container) =
            world.On<Draw> <| fun e ->
                for actor in world.Query<ColliderGroup, Eid>() do
                    let struct (colliderGroup, eid) = actor.Values
                    for collider in colliderGroup.Colliders do
                        let entity = world.Get eid
                        match entity.TryGet<Transform>() with
                        | true, {Position=Position pos} ->
                            let color = if collider.IsTrigger then Color.Magenta else Color.Yellow
                            e.SpriteBatch.DrawRectangle(collider.ColliderBounds.OffsetValue(pos).ToRectangleF(), color, 3f)
                        | false, _ ->
                            e.SpriteBatch.DrawRectangle(collider.ColliderBounds.ToRectangleF(), Color.Green, 3f)


let configure (world: Container) =
      [ Systems.draw world ]