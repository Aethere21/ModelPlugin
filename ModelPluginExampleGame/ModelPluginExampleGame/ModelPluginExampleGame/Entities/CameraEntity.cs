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
            //RotationEnabled and MovementEnabled are defined in glue.
            ///Problem with rotation: Setting RotationX while RotationEnabled is true.
            if (RotationEnabled)
            { CameraRotation(); }
            if (MovementEnabled)
            { CameraMovement(); }
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
            this.CameraInstance.RelativeRotationMatrix = Matrix.Identity;
            //Y axis = Up
            this.CameraInstance.UpVector = new Vector3(0, 1, 0);
            CameraInstance.CameraCullMode = FlatRedBall.Graphics.CameraCullMode.None;
            //How far the camera can see
            CameraInstance.FarClipPlane = 10000.0f;
            //Set originalMouseState to center of screen, used for camera rotation.
            Microsoft.Xna.Framework.Input.Mouse.SetPosition(FlatRedBallServices.Game.GraphicsDevice.Viewport.Width / 2,
                FlatRedBallServices.Game.GraphicsDevice.Viewport.Height / 2);
            originalMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        /// <summary>
        /// Free camera movement.
        /// </summary>
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
                this.Position += new Vector3(0, 1, 0) * TimeManager.SecondDifference * -CameraMovementSpeed;
            if (InputManager.Keyboard.KeyDown(Keys.E)) //Up
                this.Position += new Vector3(0, 1, 0) * TimeManager.SecondDifference * CameraMovementSpeed;
        }

        float clampRotationX;
        ///// <summary>
        ///// First person camera rotation.
        ///// </summary>
        private void CameraRotation()
        {
            MouseState currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if(currentMouseState != originalMouseState)
            {
                //Get amount of change
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;

                //Set clampRotationX and the actual rotation value for Y.
                //CameraRotationSpeed is defined in glue.
                RotationY += (CameraRotationSpeed * -xDifference * TimeManager.SecondDifference) / TimeManager.SecondDifference;
                clampRotationX += (CameraRotationSpeed * -yDifference * TimeManager.SecondDifference) / TimeManager.SecondDifference;

                //Set a max and min rotation for X-axis (up and down) rotation.
                if (clampRotationX > 0)
                {
                    //Clamp to Min:-10 Max:80
                    clampRotationX = MathHelper.Clamp(clampRotationX, -0.1745329252f, 1.3962634016f);
                }
                else if (clampRotationX < 0)
                {
                    //Clamp to Min:-80 Max:10
                    clampRotationX = MathHelper.Clamp(clampRotationX, -1.3962634016f, 0.1745329252f);
                }
                //Set the clamped float to actual rotation float.
                RotationX = clampRotationX;
                //Set mouse position back to the center.
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(FlatRedBallServices.GraphicsDevice.Viewport.Width / 2, FlatRedBallServices.GraphicsDevice.Viewport.Height / 2);
            }
        }

        //    private void CameraRotation()
        //    {
        //        MouseState currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        //        if (currentMouseState != originalMouseState)
        //        {
        //            float xDifference = currentMouseState.X - originalMouseState.X;
        //            float yDifference = currentMouseState.Y - originalMouseState.Y;
        //            this.RotationY += (CameraRotationSpeed * -xDifference * TimeManager.SecondDifference) / TimeManager.SecondDifference;
        //            this.RotationX += (CameraRotationSpeed * -yDifference * TimeManager.SecondDifference) / TimeManager.SecondDifference;
        //            if (RotationX > 0 && RotationX < MathHelper.ToRadians(95))
        //            {
        //                RotationX = MathHelper.Clamp(RotationX, MathHelper.ToRadians(-10), MathHelper.ToRadians(80));
        //            }
        //            else if (RotationX > 0 && RotationX > MathHelper.ToRadians(260))
        //            {
        //                RotationX = MathHelper.Clamp(RotationX, MathHelper.ToRadians(280), MathHelper.ToRadians(999));
        //            }
        //            //if (RotationX < MathHelper.ToRadians(110) && RotationX > 0)
        //            //{
        //            //    RotationX = MathHelper.Clamp(RotationX, MathHelper.ToRadians(0), MathHelper.ToRadians(80));
        //            //}
        //            //else if (RotationX > MathHelper.ToRadians(110) && RotationX > 0)
        //            //{
        //            //    RotationX = MathHelper.Clamp(RotationX, MathHelper.ToRadians(280), MathHelper.ToRadians(360));
        //            //}
        //            Microsoft.Xna.Framework.Input.Mouse.SetPosition(FlatRedBallServices.GraphicsDevice.Viewport.Width / 2, FlatRedBallServices.GraphicsDevice.Viewport.Height / 2);
        //        }
        //        FlatRedBall.Debugging.Debugger.Write("RotationX: " + MathHelper.ToDegrees(RotationX) + "\nRotationX: " + RotationX);
        //    }
        //}
    }
}
