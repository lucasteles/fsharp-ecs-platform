namespace Game.Components

open Microsoft.Xna.Framework

[<Struct>]type Collider = {ColliderBounds: Rectangle
                           IsTrigger: bool}

module Collider =
    let fromRect rect =  {ColliderBounds = rect; IsTrigger=false}
    let trigger rect =  {ColliderBounds = rect; IsTrigger=true}
    let create offset size =  rect offset size |> fromRect
