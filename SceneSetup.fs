module SceneSetup
open Game
open Game.Components
open Game.Scenes
open Game.Systems

let scenes () = [
        Scene.create SceneName.Play [ Input.configure
                                      Gravity.configure
                                      Player.configure
                                      Exit.configure
                                      Obstacles.configure
                                      ColliderDebug.configure
                                      Collision.configure
                                      ]

    ]


