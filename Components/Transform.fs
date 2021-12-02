module Game.Components.Transform

open Microsoft.Xna.Framework

[<Struct>] type Position = Position of Vector2
module Position =
    let inline create x y = vector2 x y |> Position
[<Struct>] type Rotation =  Rotation of float32
[<Struct>] type Scale =  Scale of float32
[<Struct>] type Velocity = Velocity of Vector2
module Velocity =
    let create x y = vector2 x y |> Velocity

