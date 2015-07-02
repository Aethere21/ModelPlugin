using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using FlatRedBall.Glue;
using FlatRedBall.Glue.Plugins.ExportedInterfaces;
using FlatRedBall.Glue.Plugins.Interfaces;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.VSHelpers;
using System.Reflection;
using FlatRedBall.Glue.Elements;
using FlatRedBall.IO;

namespace ModelPlugin
{
    [Export(typeof(PluginBase))]
    public class ModelPlugin : PluginBase
    {
        [Import("GlueCommands")]
        public IGlueCommands GlueCommands
        {
            get;
            set;
        }
		
		[Import("GlueState")]
		public IGlueState GlueState
		{
		    get;
		    set;
        }

        CodeBuildItemAdder itemAdder = new CodeBuildItemAdder();

        public override Version Version
        {
            get { return new Version(0, 7); }
        }

        public override string FriendlyName
        {
            get { return "Model Plugin"; }
        }

        public override bool ShutDown(PluginShutDownReason reason)
        {
            // Do anything your plugin needs to do to shut down
            // or don't shut down and return false
            return true;
        }

        public override void StartUp()
        {
            AvailableAssetTypes.Self.AddAssetType(new AssetTypeInfo
            {
                FriendlyName = "ModelDrawableBatch",
                CanBeObject = true,
                QualifiedRuntimeTypeName = new PlatformSpecificType { Platform = "All", QualifiedType = "ModelPlugin.ModelDrawableBatch" },
                AddToManagersMethod = new List<String> { "this.AddToManagers()" },
                //LayeredAddToManagersMethod = new List<String> { "this.AddToManagersLayered(mLayer)" },
                DestroyMethod = "this.Destroy()",
                ShouldAttach = true,
                MustBeAddedToContentPipeline = false,
                CanBeCloned = true,
                HasCursorIsOn = false,
                HasVisibleProperty = false, //Maybe make true?
                CanIgnorePausing = false,
                ExtraVariablesPattern = "Microsoft.Xna.Framework.Graphics.Model Model; float X; float Y; float Z; float ScaleX; float ScaleY; float ScaleZ; float RotationX; float RotationY; " +
                "float RotationZ; float Alpha; Color BlendFactor; bool TextureEnabled; Texture2D ModelTexture; bool VertexColorEnabled; " +
                "bool LightingEnabled; bool PreferPerPixelLighting; bool FogEnabled; float FogStart; float FogEnd; float FogColorR; float FogColorG; float FogColorB;",
            });
            
            itemAdder.Add("ModelDrawableBatch.cs");
            itemAdder.OutputFolderInProject = "ModelPlugin";
            ReactToLoadedGlux += HandleGluxLoad;
        }

        private void HandleGluxLoad()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            itemAdder.PerformAddAndSave(assembly);
        }
    }
}
