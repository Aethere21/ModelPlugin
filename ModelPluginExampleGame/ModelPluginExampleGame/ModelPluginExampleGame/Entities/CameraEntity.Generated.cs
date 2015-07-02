#if ANDROID || IOS
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif

using Color = Microsoft.Xna.Framework.Color;

namespace ModelPluginExampleGame.Entities
{
	public partial class CameraEntity : FlatRedBall.PositionedObject, FlatRedBall.Graphics.IDestroyable
	{
        // This is made static so that static lazy-loaded content can access it.
        public static string ContentManagerName
        {
            get;
            set;
        }

		// Generated Fields
		#if DEBUG
		static bool HasBeenLoadedWithGlobalContentManager = false;
		#endif
		static object mLockObject = new object();
		static System.Collections.Generic.List<string> mRegisteredUnloads = new System.Collections.Generic.List<string>();
		static System.Collections.Generic.List<string> LoadedContentManagers = new System.Collections.Generic.List<string>();
		
		private FlatRedBall.Camera CameraInstance;
		public float CameraRotationSpeed = 0.003f;
		public float CameraMovementSpeed = 30f;
		public bool RotationEnabled = true;
		public bool MovementEnabled = true;
		protected FlatRedBall.Graphics.Layer LayerProvidedByContainer = null;

        public CameraEntity()
            : this(FlatRedBall.Screens.ScreenManager.CurrentScreen.ContentManagerName, true)
        {

        }

        public CameraEntity(string contentManagerName) :
            this(contentManagerName, true)
        {
        }


        public CameraEntity(string contentManagerName, bool addToManagers) :
			base()
		{
			// Don't delete this:
            ContentManagerName = contentManagerName;
            InitializeEntity(addToManagers);

		}

		protected virtual void InitializeEntity(bool addToManagers)
		{
			// Generated Initialize
			LoadStaticContent(ContentManagerName);
			CameraInstance = FlatRedBall.SpriteManager.Camera;
			
			PostInitialize();
			if (addToManagers)
			{
				AddToManagers(null);
			}


		}

// Generated AddToManagers
		public virtual void ReAddToManagers (FlatRedBall.Graphics.Layer layerToAddTo)
		{
			LayerProvidedByContainer = layerToAddTo;
			FlatRedBall.SpriteManager.AddPositionedObject(this);
		}
		public virtual void AddToManagers (FlatRedBall.Graphics.Layer layerToAddTo)
		{
			LayerProvidedByContainer = layerToAddTo;
			FlatRedBall.SpriteManager.AddPositionedObject(this);
			AddToManagersBottomUp(layerToAddTo);
			CustomInitialize();
		}

		public virtual void Activity()
		{
			// Generated Activity
			
			CustomActivity();
			
			// After Custom Activity
		}

		public virtual void Destroy()
		{
			// Generated Destroy
			FlatRedBall.SpriteManager.RemovePositionedObject(this);
			


			CustomDestroy();
		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			if (CameraInstance.Parent == null)
			{
				CameraInstance.CopyAbsoluteToRelative();
				CameraInstance.AttachTo(this, false);
			}
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public virtual void AddToManagersBottomUp (FlatRedBall.Graphics.Layer layerToAddTo)
		{
			AssignCustomVariables(false);
		}
		public virtual void RemoveFromManagers ()
		{
			FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
		}
		public virtual void AssignCustomVariables (bool callOnContainedElements)
		{
			if (callOnContainedElements)
			{
			}
			CameraRotationSpeed = 0.003f;
			CameraMovementSpeed = 30f;
			RotationEnabled = true;
			MovementEnabled = true;
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			this.ForceUpdateDependenciesDeep();
			FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
		}
		public static void LoadStaticContent (string contentManagerName)
		{
			if (string.IsNullOrEmpty(contentManagerName))
			{
				throw new System.ArgumentException("contentManagerName cannot be empty or null");
			}
			ContentManagerName = contentManagerName;
			#if DEBUG
			if (contentManagerName == FlatRedBall.FlatRedBallServices.GlobalContentManager)
			{
				HasBeenLoadedWithGlobalContentManager = true;
			}
			else if (HasBeenLoadedWithGlobalContentManager)
			{
				throw new System.Exception("This type has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
			}
			#endif
			bool registerUnload = false;
			if (LoadedContentManagers.Contains(contentManagerName) == false)
			{
				LoadedContentManagers.Add(contentManagerName);
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("CameraEntityStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
			}
			if (registerUnload && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
			{
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("CameraEntityStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
			}
			CustomLoadStaticContent(contentManagerName);
		}
		public static void UnloadStaticContent ()
		{
			if (LoadedContentManagers.Count != 0)
			{
				LoadedContentManagers.RemoveAt(0);
				mRegisteredUnloads.RemoveAt(0);
			}
			if (LoadedContentManagers.Count == 0)
			{
			}
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			return null;
		}
		public static object GetFile (string memberName)
		{
			return null;
		}
		object GetMember (string memberName)
		{
			return null;
		}
		protected bool mIsPaused;
		public override void Pause (FlatRedBall.Instructions.InstructionList instructions)
		{
			base.Pause(instructions);
			mIsPaused = true;
		}
		public virtual void SetToIgnorePausing ()
		{
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(this);
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(CameraInstance);
		}
		public virtual void MoveToLayer (FlatRedBall.Graphics.Layer layerToMoveTo)
		{
			LayerProvidedByContainer = layerToMoveTo;
		}

    }
	
	
	// Extra classes
	
}
