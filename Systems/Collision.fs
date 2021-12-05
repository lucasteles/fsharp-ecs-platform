module Game.Systems.Collision

open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework



module private Systems =
    let collide (world: Container) =
            world.On<Update> <| fun e ->
                let actors = world.Query<Transform, Velocity, Collider, Eid>()
                let staticColliders = world.Query<Collider, Eid>()
                let colliders = world.Query<Collider, Transform, Eid>()
                for actor in actors do
                    let struct ({ Position=(Position position) } as transform,
                                (Velocity velocity),
                                actorCollider,
                                eid) = actor.Values
                    for collider in staticColliders do
                        let struct (collider, colliderEid) = collider.Values
                        if (eid <> colliderEid
                            && (actorCollider.ColliderBounds.OffsetValue(position)).Intersects(collider.ColliderBounds))
                        then
                            let comp = &actor.Value1
                            comp <- {  transform with Position = Position (position - velocity) }
                    for collider in colliders do
                        let struct (collider, {Position=(Position colliderPosition)}, colliderEid) = collider.Values
                        if (eid <> colliderEid
                            && (actorCollider.ColliderBounds.OffsetValue(position)).Intersects(collider.ColliderBounds.OffsetValue(colliderPosition)))
                        then
                            let comp = &actor.Value1
                            comp <- {  transform with Position = Position (position - velocity) }

let configure (world: Container) =
      [ Systems.collide world ]