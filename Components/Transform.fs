module Game.Components.Transform

open System
open System.Drawing
open System.Numerics
open System.Numerics
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

[<Measure>] type Radians

[<Struct>] type Position = Position of Vector2
module Position =
    let inline create x y = vector2 x y |> Position
[<Struct>] type Rotation =  Rotation of single<Radians>
[<Struct>] type Scale = Scale of Vector2
[<Struct>] type ScalarScale = ScalarScale of single

[<Struct>] type Transform =
              { Position: Position
                Scale: Scale
                Rotation: Rotation }
module Transform =
    let create (position, rotation, scale) =
        { Position = position
          Rotation = rotation
          Scale = scale }
    let tuple { Position = position
                Rotation = rotation
                Scale =  scale } =
        struct (position, rotation, scale)

    let tupleValues { Position = (Position position)
                      Rotation = (Rotation rotation)
                      Scale =  (Scale scale) } =
        struct (position, rotation, scale)

[<Struct>] type Velocity = Velocity of Vector2
module Velocity =
    let create x y = vector2 x y |> Velocity


[<AutoOpen>]
module SpriteBatch =
    type SpriteBatch with
        member this.Draw(texture , Position position, sourceRectangle, color, Rotation rotation, origin, (Scale scale), effects,layerDepth) =
           this.Draw(texture, position, sourceRectangle, color, rotation / 1f<Radians>, origin, scale, effects, layerDepth)
        member this.Draw(texture , Position position, sourceRectangle, color, Rotation rotation, origin, (ScalarScale scale), effects,layerDepth) =
           this.Draw(texture, position, sourceRectangle, color, rotation / 1f<Radians>, origin, scale, effects, layerDepth)
        member this.Draw(texture, { Position=Position position; Rotation=Rotation rotation; Scale=Scale scale  }, sourceRectangle, color, origin, effects,layerDepth) =
           this.Draw(texture, position, sourceRectangle, color, rotation / 1f<Radians>, origin, scale, effects, layerDepth)
        member this.Draw(texture, Position position, sourceRectangle, color) =
           this.Draw(texture, position, ValueOption.toNullable sourceRectangle, color)
        member this.Draw(texture: Texture2D, destinationRectangle: Rectangle, sourceRectangle:ValueOption<Rectangle>, color) =
            this.Draw(texture, destinationRectangle, ValueOption.toNullable sourceRectangle, color)
        member this.Draw(texture, (Position position), color) =
            this.Draw(texture, position, color)
