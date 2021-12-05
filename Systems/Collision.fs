module Game.Systems.Collision

open System
open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework


module private Systems =
    let collide (world: Container) =
            world.On<Update> <| fun e ->
                let actors = world.Query<Transform, Velocity, Collider, Eid>()
                let colliders = world.Query<Collider, Eid>()
                for actor in actors do
                    let struct ({ Position=(Position position) } as transform,
                                Velocity velocity,
                                actorCollider,
                                eid) = actor.Values
                    for collider in colliders do
                        let struct (collider, colliderEid) = collider.Values
                        let entity = world.Get colliderEid
                        let colliderRectangle =
                            match entity.TryGet<Transform>() with
                            | true, {Position=Position pos} -> collider.ColliderBounds.OffsetValue(pos)
                            | false, _ -> collider.ColliderBounds
                        let actorRectangle = actorCollider.ColliderBounds.OffsetValue(position)
                        if (eid <> colliderEid && actorRectangle.Intersects(colliderRectangle)) then

                            let overlap = Rectangle.Intersect(actorRectangle, colliderRectangle)
                            let rawDirection = actorRectangle.Center.ToVector2().Direction(overlap.Center.ToVector2())
                            let dir = vector2 (MathF.Round(rawDirection.X)) (MathF.Round(rawDirection.Y))
                            let transformRef = &actor.Value1
                            transformRef <- {  transform
                                               with Position = Position (position - overlap.Size.ToVector2() * dir ) }

                            let velocityRef = &actor.Value2
                            velocityRef <- Velocity (velocity - velocity * dir)
                            world.Send({ Game = e.Game
                                         From = eid
                                         FromBounds = actorRectangle
                                         Other = colliderEid
                                         Bounds = colliderRectangle })

let configure (world: Container) =
      [ Systems.collide world ]