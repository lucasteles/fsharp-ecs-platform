module SceneSetup
open Game
open Game.Scenes
open Game.Systems

let scenes () = [
        Scene.create SceneName.Play [ Input.configure
                                      Player.configure
                                      Exit.configure ]

    ]


