using System;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;

using strangeetnix.game;

namespace strangeetnix.ui
{
	public class SwitchCanvasCommand : Command
	{
		[Inject (ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{ get; set; }

		[Inject]
		public UIStates state{ get; set; }

		public override void Execute ()
		{
			if (state == UIStates.GAME) {
				destroyCanvas (AssetConfig.CANVAS_MAIN.id);
				createCanvas (AssetConfig.CANVAS_GAME);
			} else {
				destroyCanvas (AssetConfig.CANVAS_GAME.id);
				createCanvas (AssetConfig.CANVAS_MAIN);
			}
		}

		private void destroyCanvas(string name)
		{
			GameObject oldState = GameObject.Find (name);
			if (oldState) {
				GameObject.Destroy (oldState);
			}
		}

		private void createCanvas(AssetPathData assetPathData)
		{
			GameObject canvasStyle;
			GameObject canvasGO;
			canvasStyle = Resources.Load<GameObject> (assetPathData.path);
			canvasGO = GameObject.Instantiate (canvasStyle) as GameObject;
			canvasGO.name = assetPathData.id;
			canvasGO.transform.SetParent(contextView.transform, false);
		}
	}
}

