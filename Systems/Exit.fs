module Game.Systems.Exit

    open Game.Events
    open Garnet.Composition
    open Microsoft.Xna.Framework
    open Microsoft.Xna.Framework.Input

    let configure (world: Container) = [
        // quit game system
        world.On<Update>
            (fun e ->
                if GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
                   || Keyboard.GetState().IsKeyDown(Keys.Escape)
                then e.Game.Exit())
        ]