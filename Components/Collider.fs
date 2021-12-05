namespace Game.Components

open Microsoft.Xna.Framework



[<Struct>]type Collider = {ColliderBounds: Rectangle
                           IsTrigger: bool
                           mutable Entered: bool}
[<Struct>]type ColliderGroup = {Colliders: Collider[]}

module ColliderGroup =
    let single collider = {Colliders=[|collider|]}

module Collider =
    let fromRect rect =  {ColliderBounds = rect; IsTrigger=false; Entered = false} |> ColliderGroup.single
    let triggerRect rect =  {ColliderBounds = rect; IsTrigger=true; Entered = false} |> ColliderGroup.single
    let create offset size =  rect offset size |> fromRect
    let trigger offset size =  rect offset size |> triggerRect
    let group colliders = {Colliders=colliders |> Array.collect (fun x -> x.Colliders)}
