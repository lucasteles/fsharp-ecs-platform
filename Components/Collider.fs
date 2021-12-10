namespace Game.Components

open System
open Microsoft.Xna.Framework



[<Struct>]type Collider = {Name: string
                           ColliderBounds: Rectangle
                           IsTrigger: bool}
[<Struct>]type ColliderGroup = {Colliders: Collider[]}

module ColliderGroup =
    let single collider = {Colliders=[|collider|]}

module Collider =
    let fromRectNamed name rect =  {Name = name; ColliderBounds = rect; IsTrigger=false} |> ColliderGroup.single
    let fromRect rect =  {Name = (Guid.NewGuid() |> string); ColliderBounds = rect; IsTrigger=false} |> ColliderGroup.single
    let triggerRect name rect =  {Name = name; ColliderBounds = rect; IsTrigger=true} |> ColliderGroup.single
    let createNamed name offset size = rect offset size |> fromRectNamed name
    let create offset size = rect offset size |> fromRect
    let trigger name offset size =  rect offset size |> triggerRect name
    let group colliders = {Colliders=colliders |> Array.collect (fun x -> x.Colliders)}
