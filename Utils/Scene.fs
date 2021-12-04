namespace Game

open System
open System.ComponentModel
open Game.Scenes
open Garnet.Composition
open Microsoft.Xna.Framework

type Scene =
    private {
        SceneName: SceneName
        World: Container
        Disposable: IDisposable
        Game: Game
    }
    interface IDisposable with
        member this.Dispose() = this.Disposable.Dispose()

module Scene =
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
    let dispose scene =
        scene.Disposable.Dispose()
        scene.World.DestroyAll()
        scene.World.UnsubscribeAll()
