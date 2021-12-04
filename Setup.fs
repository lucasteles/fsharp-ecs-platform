module GameLogic
open Game
open Game.Systems

type SceneName = Play


let scenes () = [
        Scene.create SceneName.Play [ Input.configure
                                      Player.configure
                                      Exit.configure ]

    ]


