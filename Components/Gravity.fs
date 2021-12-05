namespace Game.Components

open Game.Components

[<Struct>] type Gravity = Gravity of single

module Gravity =
    let G = 9.8f
    let default' = Gravity G
