using FlatRedBall.Graphics;
using FlatRedBall;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ModelPlugin
{
    public class ModelDrawableBatch : PositionedObject, IDrawableBatch
    {
        #region Properties
        public Model Model { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public Texture2D ModelTexture { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public BlendState BlendState { get; set; }
        public float Alpha { get; set; }
        public bool FogEnabled { get; set; }
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public float FogColorR { get; set; }
        public float FogColorG { get; set; }
        public float FogColorB { get; set; }
        public bool TextureEnabled { get; set; }
        public bool VertexColorEnabled { get; set; }
        public bool PreferPerPixelLighting { get; set; }
        public bool EnableDefaultLighting { get; set; }
        public bool LightingEnabled { get; set; }
        public Color BlendFactor { get; set; }

        public bool UpdateEveryFrame
        {
            get { return false; }
        }
        #endregion

        #region Initializers
        public ModelDrawableBatch()
        {
            SetDefaultValues();
        }
        public void SetDefaultValues()
        {
            ScaleX = 1;
            ScaleY = 1;
            ScaleZ = 1;
            RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.CullCounterClockwiseFace,
                FillMode = FillMode.Solid
            };
            VertexColorEnabled = false;
            TextureEnabled = false;
            ModelTexture = null;
            BlendState = BlendState.AlphaBlend;
            BlendFactor = new Color(255, 255, 255, 255);
            DepthStencilState = DepthStencilState.Default;
            LightingEnabled = true;
            EnableDefaultLighting = true;
            PreferPerPixelLighting = true;
            Alpha = 1;
            FogEnabled = false;
        }
        #endregion

        #region Methods
        public void AddToManagers()
        {
            SpriteManager.AddZBufferedDrawableBatch(this);
            SpriteManager.AddPositionedObject(this);
        }

        //public void AddToManagersLayered(Layer mLayer)
        //{
        //    SpriteManager.AddZBufferedDrawableBatch(this);
        //    SpriteManager.AddPositionedObject(this);
        //    SpriteManager.AddToLayer(this, layer);
        //}

        public void Destroy()
        {
            SpriteManager.RemoveDrawableBatch(this);
            SpriteManager.RemovePositionedObject(this);
        }

        public void Update()
        {

        }

        public void Draw(Camera camera)
        {
            Matrix[] _transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(_transforms);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        ///Position of model in the world.
                        effect.World = _transforms[mesh.ParentBone.Index] * Matrix.CreateScale(new Vector3(ScaleX, ScaleY, ScaleZ)) * Matrix.CreateFromYawPitchRoll(this.RotationY, this.RotationX, this.RotationZ) * Matrix.CreateTranslation(this.Position);
                        effect.View = Camera.Main.View;
                        effect.Projection = Camera.Main.Projection;

                        effect.LightingEnabled = LightingEnabled;
                        if (EnableDefaultLighting)
                            effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = PreferPerPixelLighting;

                        effect.TextureEnabled = TextureEnabled;
                        ///If the texture of the model is not null, set the texture to the assigned texture.
                        if (ModelTexture != null)
                        {
                            effect.Texture = ModelTexture;
                        }

                        effect.Alpha = Alpha;

                        effect.FogEnabled = FogEnabled;
                        effect.FogColor = new Vector3(FogColorR, FogColorG, FogColorB);
                        effect.FogStart = FogStart;
                        effect.FogEnd = FogEnd;

                        effect.GraphicsDevice.RasterizerState = RasterizerState;
                        effect.GraphicsDevice.DepthStencilState = DepthStencilState;
                        effect.GraphicsDevice.BlendState = BlendState;
                        effect.GraphicsDevice.BlendFactor = BlendFactor;
                        //SamplerState(???)
                    }
                    mesh.Draw();
                }
            }
        }
        #endregion
    }
}
