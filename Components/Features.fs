namespace Game.Components

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

type PlayerButtomState = Pressed | Released

type PlayerState = Idle | Jump

[<Struct>]
type PlayerInput = { Direction: Vector2; Jump: PlayerButtomState }
    with static member zero = {Direction = Vector2.Zero; Jump = PlayerButtomState.Released}

[<Struct>]
type Player = {
    Texture: Texture2D
    Size: Vector2
    Speed: single
    JumpForce: single
    PlayerState: PlayerState
}

[<Struct>]type Wall = { WallTexture: Texture2D }