[<AutoOpen>]
module VectorModule
open Microsoft.Xna.Framework

let inline vector2 x y = Vector2(single x, single y)

let rect (location:Vector2) (size:Vector2) = Rectangle(location.ToPoint(), size.ToPoint())
module Vector2 =
    let toTuple (vector: Vector2) = vector.X, vector.Y
    let normalize (vector: Vector2) =
        if (vector <> Vector2.Zero) then vector.Normalize()
        vector
    let Up = vector2 0 -1
    let Down = vector2 0 1
    let Left = vector2 -1 0
    let Right = vector2 1 0

type Vector2 with
    member inline this.WithX x = vector2 x this.Y
    member inline this.WithY y = vector2 this.X y
    member this.Direction(other: Vector2) =
        (other-this) |> Vector2.normalize

let (|Vec|_|) v = v |> Vector2.toTuple |> Some
