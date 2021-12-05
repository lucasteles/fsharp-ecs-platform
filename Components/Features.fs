namespace Game.Components

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

[<Struct>]
type PlayerInput = { Direction: Vector2 }
    with static member zero = {Direction = Vector2.Zero}

[<Struct>]
type Player = {
    Texture: Texture2D
    Size: Vector2
    Speed: single
}

[<Struct>]type Wall = { WallTexture: Texture2D }