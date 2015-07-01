#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

#endif
#endregion

namespace ModelPluginExampleGame.Entities
{
	public partial class CameraEntity
	{
        private MouseState originalMouseState;        

		private void CustomInitialize()
		{
            SetUpCamera();
		}

		private void CustomActivity()
		{
            //Camera movement and rotation need to be fixed, although works for now!
            CameraMovement();
            CameraRotation();
        }

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        private void SetUpCamera()
        {
            this.CameraInstance.RelativePosition = new Vector3(0, 0, 0);
            //Y axis = Up
            this.CameraInstance.UpVector = new Vector3(0, 1, 0);
            CameraInstance.CameraCullMode = FlatRedBall.Graphics.CameraCullMode.None;

            //How far the camera can see
            CameraInstance.FarClipPlane = 10000.0f;
            //Set originalMouseState to center of screen
            Microsoft.Xna.Framework.Input.Mouse.SetPosition(FlatRedBallServices.Game.GraphicsDevice.Viewport.Width / 2,
                FlatRedBallServices.Game.GraphicsDevice.Viewport.Height / 2);
            originalMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }


        private void CameraMovement()
        {
            //CameraMovementSpeed is defined in glue.
            if (InputManager.Keyboard.KeyDown(Keys.W)) //Forward
                this.Position += RotationMatrix.Forward * TimeManager.SecondDifference * CameraMovementSpeed;
            if (InputManager.Keyboard.KeyDown(Keys.S)) //Backward
                this.Position += RotationMatrix.Forward * TimeManager.SecondDifference * -CameraMovementSpeed;
            if (InputManager.Keyboard.KeyDown(Keys.A)) //Left
                this.Position += RotationMatrix.Right * TimeManager.SecondDifference * -CameraMovementSpeed;
            if (InputManager.Keyboard.KeyDown(Keys.D)) //Right
                this.Position += RotationMatrix.Right * TimeManager.SecondDifference * CameraMovementSpeed;
            if (InputManager.Keyboard.KeyDown(Keys.Q)) //Down
                this.Position += RotationMatrix.Up * TimeManager.SecondDifference * -CameraMovementSpeed;
            if (InputManager.Keyboard.KeyDown(Keys.E)) //Up
                this.Position += RotationMatrix.Up * TimeManager.SecondDifference * CameraMovementSpeed;
        }

        private void CameraRotation()
        {
            //Gets current position of mouse
            MouseState currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            //If the current position of the mouse is not centered, originalMouseState, continue on.
            if (currentMouseState != originalMouseState)
            {
                //Gets the difference in mouse position to use to apply to rotation matrix.
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;

                Vector3 absoluteYAxis = new Vector3(0, 1, 0);
                absoluteYAxis.Normalize();
                Vector3 relativeXAxis = this.RotationMatrix.Right;
                relativeXAxis.Normalize();

                //CameraRotationSpeed is defined in glue.
                //Applies rotation to the rotation matrix.
                //X
                this.RotationMatrix *= Matrix.CreateFromAxisAngle(absoluteYAxis, xDifference * -CameraRotationSpeed * TimeManager.SecondDifference);
                //Y
                this.RotationMatrix *= Matrix.CreateFromAxisAngle(relativeXAxis, yDifference * -CameraRotationSpeed * TimeManager.SecondDifference);

                //Sets the mouse position back to the center.
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(FlatRedBallServices.GraphicsDevice.Viewport.Width / 2, FlatRedBallServices.GraphicsDevice.Viewport.Height / 2);
            }
        }

	}
}
