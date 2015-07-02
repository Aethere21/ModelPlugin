using System;
using System.Collections.Generic;
using System.Reflection;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;

using System.Linq;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FlatRedBall.Input;

namespace ModelPluginExampleGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public Game1() : base ()
        {
            graphics = new GraphicsDeviceManager(this);

#if WINDOWS_PHONE || ANDROID || IOS

			// Frame rate is 30 fps by default for Windows Phone,
            // so let's keep that for other phones too
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            graphics.IsFullScreen = true;
#elif WINDOWS
            graphics.PreferredBackBufferHeight = 600;
#endif


#if WINDOWS_8
            FlatRedBall.Instructions.Reflection.PropertyValuePair.TopLevelAssembly = 
                this.GetType().GetTypeInfo().Assembly;
#endif

        }

        protected override void Initialize()
        {
			#if IOS
			var bounds = UIKit.UIScreen.MainScreen.Bounds;
			var nativeScale = UIKit.UIScreen.MainScreen.Scale;
			var screenWidth = (int)(bounds.Width * nativeScale);
			var screenHeight = (int)(bounds.Height * nativeScale);
			graphics.PreferredBackBufferWidth = screenWidth;
			graphics.PreferredBackBufferHeight = screenHeight;
			#endif
		
            FlatRedBallServices.InitializeFlatRedBall(this, graphics);
			CameraSetup.SetupCamera(SpriteManager.Camera, graphics);
			GlobalContent.Initialize();

			FlatRedBall.Screens.ScreenManager.Start(typeof(ModelPluginExampleGame.Screens.GameScreen));

            base.Initialize();
        }

        bool cap = true;
        protected override void Update(GameTime gameTime)
        {
            FlatRedBallServices.Update(gameTime);

            FlatRedBall.Screens.ScreenManager.Activity();

            if (InputManager.Keyboard.KeyReleased(Keys.V))
            {
                cap = !cap;
                IsFixedTimeStep = cap;
                graphics.SynchronizeWithVerticalRetrace = cap;
                graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            FlatRedBallServices.Draw();

            base.Draw(gameTime);
        }
    }
}
