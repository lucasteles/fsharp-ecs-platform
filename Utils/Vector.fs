[<AutoOpen>]
module VectorModule
open Microsoft.Xna.Framework

type double = float
type float = float32

let inline vector2 x y = Vector2(single x, single y)

let rect (location:Vector2) (size:Vector2) = Rectangle(location.ToPoint(), size.ToPoint())
module Vector2 =
    let toTuple (vector: Vector2) = vector.X, vector.Y
    let normalize (vector: Vector2) =
        if (vector <> Vector2.Zero) then vector.Normalize()
        vector

type Vector2 with
    member this.WithX x = vector2 x this.Y
    member this.WithY y = vector2 this.X y

let (|Vec|_|) v = v |> Vector2.toTuple |> Some
