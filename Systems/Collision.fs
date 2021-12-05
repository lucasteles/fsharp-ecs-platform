module Game.Systems.Collision

open System
open Game.Components
open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework

let private updateEnteredColliderGroup colliderGroup collider entered =
    {
        colliderGroup with
            Colliders =
            [|
                for c in colliderGroup.Colliders do
                    if (c = collider)
                    then  { c with Entered = entered }
                    else c
            |]
    }

module private Systems =
    let collide (world: Container) =
            world.On<Update> <| fun e ->
                let actors = world.Query<Transform, Velocity, ColliderGroup, Eid>()
                let colliders = world.Query<ColliderGroup, Eid>()
                for actor in actors do
                    let struct ({ Position=(Position position) } as transform,
                                Velocity velocity,
                                actorColliderGroup,
                                eid) = actor.Values
                    for actorCollider in actorColliderGroup.Colliders do
                        for other in colliders do
                            let struct (colliderGroup, colliderEid) = other.Values
                            for collider in colliderGroup.Colliders do
                                let colliderEntity = world.Get colliderEid
                                let colliderRectangle =
                                    match colliderEntity.TryGet<Transform>() with
                                    | true, {Position=Position pos} -> collider.ColliderBounds.OffsetValue(pos)
                                    | false, _ -> collider.ColliderBounds
                                let actorRectangle = actorCollider.ColliderBounds.OffsetValue(position)
                                if eid <> colliderEid && actorRectangle.Intersects(colliderRectangle) then
                                    let overlap = Rectangle.Intersect(actorRectangle, colliderRectangle)
                                    if actorCollider.IsTrigger || collider.IsTrigger then
                                        if not actorCollider.Entered || not collider.Entered then
                                            world.Send({ Game = e.Game
                                                         Origin = eid
                                                         FromBounds = actorRectangle
                                                         Other = colliderEid
                                                         Bounds = colliderRectangle
                                                         Overlap = overlap
                                                         TriggerFrom= actorCollider
                                                         TriggerWith = collider })
                                    else
                                        let rawDirection = actorRectangle.Center.ToVector2().Direction(overlap.Center.ToVector2())
                                        let dir = vector2 (MathF.Round(rawDirection.X)) (MathF.Round(rawDirection.Y))
                                        let transformRef = &actor.Value1
                                        transformRef <- {  transform
                                                           with Position = Position (position - overlap.Size.ToVector2() * dir ) }

                                        let velocityRef = &actor.Value2
                                        velocityRef <- Velocity.zero
                                        world.Send({ Game = e.Game
                                                     Origin = eid
                                                     FromBounds = actorRectangle
                                                     Other = colliderEid
                                                     Bounds = colliderRectangle
                                                     Overlap = overlap
                                                     CollideFrom = actorCollider
                                                     CollideWith = collider})

//                                    if not actorCollider.Entered then
//                                        actor.Value3 <- (updateEnteredColliderGroup actorColliderGroup actorCollider true)
//                                    if not collider.Entered then
//                                        other.Value1 <- (updateEnteredColliderGroup colliderGroup collider true)
//                                else
//                                    if actorCollider.Entered then
//                                        actor.Value3 <- (updateEnteredColliderGroup actorColliderGroup actorCollider false)
//                                    if collider.Entered then
//                                        other.Value1 <- (updateEnteredColliderGroup colliderGroup collider false)

let configure (world: Container) =
      [ Systems.collide world ]