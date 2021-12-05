namespace Game.Components

open Game.Components

[<Struct>] type Gravity = Gravity of single

module Gravity =
    let G = 30f
    let default' = Gravity G
