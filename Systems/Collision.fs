module Game.Systems.Collision

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

                            let horizontal =
                                if (actorRectangle.Left >= colliderRectangle.Left
                                   && actorRectangle.Left <= colliderRectangle.Right)
                                   || (actorRectangle.Right >= colliderRectangle.Left
                                   && actorRectangle.Right <= colliderRectangle.Right)
                                   || (actorRectangle.Left <= colliderRectangle.Left
                                   && actorRectangle.Right >= colliderRectangle.Right)
                                then 1f else 0f

                            let vertical =
                                if (actorRectangle.Top >= colliderRectangle.Top
                                   && actorRectangle.Top <= colliderRectangle.Bottom)
                                   || (actorRectangle.Bottom >= colliderRectangle.Left
                                   && actorRectangle.Bottom <= colliderRectangle.Right)
                                   || (actorRectangle.Top <= colliderRectangle.Top
                                   && actorRectangle.Bottom >= colliderRectangle.Bottom)
                                then 1f else 0f

                            let transformRef = &actor.Value1
                            transformRef <- {  transform
                                               with Position =
                                                       Position (position - velocity * Vector2(horizontal, vertical) * e.DeltaTime.seconds) }

                            let velocityRef = &actor.Value2
                            velocityRef <- Velocity Vector2.Zero
                            world.Send({ Game = e.Game
                                         From = eid
                                         FromBounds = actorRectangle
                                         Other = colliderEid
                                         Bounds = colliderRectangle })

let configure (world: Container) =
      [ Systems.collide world ]