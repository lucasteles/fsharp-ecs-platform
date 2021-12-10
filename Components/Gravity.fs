namespace Game.Components

[<Struct>] type Gravity = { Force: single; MaxSpeed: single }

module Gravity =
    let G = 30f
    let Max = 1000f
    let default' = { Force = G; MaxSpeed = Max }
