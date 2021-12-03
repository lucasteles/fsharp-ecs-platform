﻿namespace TheGame

open Game.Events
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable disposables = Unchecked.defaultof<_>
    let mutable spriteBatch = Unchecked.defaultof<_>

    let world = Container()

    do this.Content.RootDirectory <- "Content"
       this.IsMouseVisible <- true
       disposables <- GameLogic.configureWorld world

    override this.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        world.Run (LoadContent this)

    override this.UnloadContent() =
        for d in disposables do d.Dispose()

    override this.Initialize() =
        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768
        //graphics.ToggleFullScreen()
        graphics.ApplyChanges()
        base.Initialize()
        world.Run (Start this)

    override this.Update gameTime =
        world.Run { DeltaTime = gameTime.ElapsedGameTime; Game = this }
        base.Update gameTime

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.Gray
        spriteBatch.Begin()
        world.Run { Time = gameTime.ElapsedGameTime
                    SpriteBatch = spriteBatch }
        spriteBatch.End()

        base.Draw(gameTime)