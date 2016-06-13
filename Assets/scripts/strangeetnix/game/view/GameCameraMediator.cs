using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class GameCameraMediator : Mediator
	{
		//View
		[Inject]
		public GameCameraView view { get; set; }

		[Inject]
		public CameraEnabledSignal cameraEnabledSignal{ get; set; }

		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			view.updateObjects ();
			//cameraEnabledSignal.AddListener (onCameraEnabled);
			//view.updateImagesSignal.AddListener (onUpdateImages);
		}

		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			//cameraEnabledSignal.RemoveListener (onCameraEnabled);
			//view.updateImagesSignal.RemoveListener (onUpdateImages);
			//view.collisionSignal.RemoveListener (onCollision);
			//gameInputSignal.RemoveListener (onGameInput);
		}

		private void onCameraEnabled(bool value)
		{
			if ((!view.enabled && value) || (view.enabled && !value)) {
				view.enabled = value;
			}
		}
	}
}

