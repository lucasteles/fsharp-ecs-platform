module Game.Scene

open System
open System.ComponentModel
open Garnet.Composition
open Microsoft.Xna.Framework

type Scene<'a> =
    private {
        SceneName: 'a
        World: Container
        Disposable: IDisposable
        Game: Game
    }
    interface IDisposable with
        member this.Dispose() = this.Disposable.Dispose()

let create sceneName systems =
    let world = Container()
    let dispose =  systems
                   |> List.map (fun fn -> fn world)
                   |> List.collect id
                   |> Disposable.Create

    fun (game:Game) ->
    {
        SceneName = sceneName
        Disposable = dispose
        World = world
        Game = game
    }

let name scene = scene.SceneName
let world scene = scene.World
let dispose scene = scene.Disposable.Dispose()
