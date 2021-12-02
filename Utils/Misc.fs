[<AutoOpen>]
module GameUtils

open System
module TimeSpan =
    let seconds (t: TimeSpan) = single t.TotalSeconds
type TimeSpan with member this.seconds = TimeSpan.seconds this

let tap x = printfn "%A" x; x
let log x = printfn "%A" x
let inline divideBy b a  = a / b