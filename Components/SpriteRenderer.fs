namespace Game.Components

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

type SpriteRenderer =
    {
        Texture: Texture2D
        Color: Color
        SourceRectangle: Rectangle
        Origin: Vector2
        Effects: SpriteEffects
        LayerDepth: single
    }
    static member create (texture, ?sourceRectangle, ?color, ?origin, ?effects,?layerDepth) =
            {
              Texture = texture
              SourceRectangle = Option.defaultValue texture.Bounds sourceRectangle
              Color = Option.defaultValue Color.White color
              Origin = Option.defaultValue Vector2.Zero origin
              Effects = Option.defaultValue SpriteEffects.None effects
              LayerDepth = Option.defaultValue 0f layerDepth
            }
