module Game.Systems.Renderer

open Game.Components
open Game.Events
open Garnet.Composition

let drawTransform (world: Container) =
    world.On<Draw> <| fun e ->
        for query in world.Query<Transform, SpriteRenderer>() do
            let struct (transform, sprite) = query.Values
            e.SpriteBatch.Draw(
                sprite.Texture,
                transform,
                sprite.SourceRectangle,
                sprite.Color,
                sprite.Origin,
                sprite.Effects,
                sprite.LayerDepth)

let drawColliders (world: Container) =
    world.On<Draw> <| fun e ->
        for query in world.Query<Collider, SpriteRenderer>() do
            let struct (collider, sprite) = query.Values
            e.SpriteBatch.Draw(
                sprite.Texture,
                collider.ColliderBounds.TopLeft,
                collider.ColliderBounds,
                sprite.Color,
                0f,
                sprite.Origin,
                1f,
                sprite.Effects,
                sprite.LayerDepth)

let configure (world: Container) =
      [drawTransform world
       drawColliders world]