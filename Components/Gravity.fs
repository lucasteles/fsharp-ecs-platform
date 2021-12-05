namespace Game.Components

open Game.Components

[<Struct>] type Gravity = Gravity of single

module Gravity =
    let G = 20f
    let default' = Gravity G
