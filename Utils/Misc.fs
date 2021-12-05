[<AutoOpen>]
module GameUtils

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open MonoGame.Extended

module TimeSpan =
    let seconds (t: TimeSpan) = single t.TotalSeconds
type TimeSpan with member this.seconds = TimeSpan.seconds this

let inline tap x = printfn "%A" x; x
let log x = printfn "%A" x
let inline divideBy b a  = a / b

let colorTexture (game: Game) color =
    let texture = new Texture2D(game.GraphicsDevice, 1, 1)
    texture.SetData([| color |])
    texture
type Rectangle with
    member this.OffsetValue(vector2:Vector2) =
              let value = this
              value.Offset(vector2)
              value
    member this.ToRectangleF() =
              RectangleF(single this.X, single this.Y, single this.Width, single this.Height)
    member this.TopLeft = Vector2(single this.Left, single this.Top)
    member this.TopRigt = Vector2(single this.Right, single this.Top)
    member this.BottomLeft = Vector2(single this.Left, single this.Bottom)
    member this.BottompRight = Vector2(single this.Right, single this.Bottom)

