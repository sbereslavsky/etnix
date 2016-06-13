using System;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace strangeetnix.ui
{
	public class SwitchCanvasCommand : Command
	{
		[Inject (ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{ get; set; }

		[Inject]
		public UIStates state{ get; set; }

		private string CANVAS_GAME = "GameCanvas";
		private string CANVAS_MAIN = "MainCanvas";
		private string FOLDER_UI   = "ui/";

		public override void Execute ()
		{
			if (state == UIStates.GAME) {
				destroyCanvas (CANVAS_MAIN);
				createCanvas (CANVAS_GAME);
			} else {
				destroyCanvas (CANVAS_GAME);
				createCanvas (CANVAS_MAIN);
			}
		}

		private void destroyCanvas(string name)
		{
			GameObject oldState = GameObject.Find (name);
			if (oldState) {
				GameObject.Destroy (oldState);
			}
		}

		private void createCanvas(string name)
		{
			GameObject canvasStyle;
			GameObject canvasGO;
			canvasStyle = Resources.Load<GameObject> (FOLDER_UI+name);
			canvasGO = GameObject.Instantiate (canvasStyle) as GameObject;
			canvasGO.name = name;
			canvasGO.transform.SetParent(contextView.transform, false);
		}
	}
}

