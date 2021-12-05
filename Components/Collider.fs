namespace Game.Components

open Microsoft.Xna.Framework

[<Struct>]type Collider = {ColliderBounds: Rectangle}

module Collider =
    let fromRect rect =  {ColliderBounds = rect}
