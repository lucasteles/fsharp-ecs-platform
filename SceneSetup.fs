module SceneSetup
open Game
open Game.Components
open Game.Scenes
open Game.Systems

let scenes () = [
        Scene.create SceneName.Play [ Exit.configure
                                      Input.configure
                                      Renderer.configure
                                      Obstacles.configure
                                      Gravity.configure
                                      Player.configure
                                      ColliderDebug.configure
                                      Collision.configure ]

    ]


