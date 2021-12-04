namespace TheGame

open Game
open Game.Events
open GameLogic
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Scene = Scene.Scene<SceneName>

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<_>

    let mutable scenes: Scene list = Unchecked.defaultof<_>
    let mutable containers: Map<SceneName, Container> = Unchecked.defaultof<_>
    let mutable currentScene = SceneName.Play

    do this.Content.RootDirectory <- "Content"
       this.IsMouseVisible <- true
       scenes <-  GameLogic.scenes() |> List.map (fun fn -> fn this)
       containers <-  scenes
                  |> List.map (fun s -> Scene.name s, Scene.world s)
                  |> Map.ofList

    override this.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        for scene in scenes do
            let world = Scene.world scene
            world.Run (LoadContent this)

    override this.UnloadContent() =
        for scene in scenes do
            Scene.dispose scene

    override this.Initialize() =
        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768
        //graphics.ToggleFullScreen()
        graphics.ApplyChanges()
        base.Initialize()
        for scene in scenes do
            let world = Scene.world scene
            world.Run (Start this)

    override this.Update gameTime =
        let world = containers.[currentScene]
        world.Run { DeltaTime = gameTime.ElapsedGameTime; Game = this }
        base.Update gameTime

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.DarkGray
        let world = containers.[currentScene]
        spriteBatch.Begin()
        world.Run { Time = gameTime.ElapsedGameTime
                    SpriteBatch = spriteBatch }
        spriteBatch.End()

        base.Draw(gameTime)
