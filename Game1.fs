namespace TheGame

open Game
open Game.Events
open Game.Scenes
open Garnet.Composition
open SceneSetup
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<_>

    let mutable scenes: Scene list = Unchecked.defaultof<_>
    let mutable containers: Map<SceneName, Container> = Unchecked.defaultof<_>
    let mutable sceneIndex = SceneName.Play

    do this.Content.RootDirectory <- "Content"
       this.IsMouseVisible <- true
       scenes <-  SceneSetup.scenes() |> List.map (fun fn -> fn this)
       containers <-  scenes
                  |> List.map (fun s -> Scene.name s, Scene.world s)
                  |> Map.ofList

    member private _.currentWorld = containers.[sceneIndex]
    override this.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        for scene in scenes do
            let world = Scene.world scene
            world.Run (LoadContent this)

    override this.UnloadContent() =
        scenes |> List.iter Scene.dispose
        this.currentWorld.Run (Stop this)
        scenes |> List.map Scene.world |> List.iter (fun c ->  c.Run (UnloadContent this))

    override this.Initialize() =
        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768
        //graphics.ToggleFullScreen()
        graphics.ApplyChanges()
        base.Initialize()
        this.currentWorld.Run (Start this)

    override this.Update gameTime =
        let world = containers.[sceneIndex]
        world.Run { DeltaTime = gameTime.ElapsedGameTime; Game = this; ChangeScene = this.ChangeScene }
        base.Update gameTime

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.DarkGray
        let world = containers.[sceneIndex]
        spriteBatch.Begin()
        world.Run { Time = gameTime.ElapsedGameTime
                    SpriteBatch = spriteBatch }
        spriteBatch.End()

        base.Draw(gameTime)

    member this.ChangeScene name =
       let oldWorld = this.currentWorld
       sceneIndex <- name
       oldWorld.Run (Stop this)
       oldWorld.DestroyAll()
       this.currentWorld.Run (Start this)
