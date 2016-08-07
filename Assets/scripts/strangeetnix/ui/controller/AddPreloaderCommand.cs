using System;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.mediation.impl;

using strangeetnix.game;

namespace strangeetnix.ui
{
	public class AddPreloaderCommand : Command
	{
		[Inject (ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{ get; set; }

		[Inject]
		public PreloaderTypes preloaderType { get; set; }

		public override void Execute ()
		{
			string canvasName = (preloaderType == PreloaderTypes.MAIN) ? AssetConfig.CANVAS_MAIN.id : AssetConfig.CANVAS_GAME.id;
			GameObject canvasGO = GameObject.Find (canvasName);
			if (canvasGO != null) {
				GameObject viewGO;
				if (preloaderType == PreloaderTypes.MAIN) {
					MainCanvasView mainCanvasView = canvasGO.GetComponent<MainCanvasView> ();
					viewGO = mainCanvasView.gameObject;
				} else {
					GameCanvasView gameCanvasView = canvasGO.GetComponent<GameCanvasView> ();
					viewGO = gameCanvasView.gameObject;
				}

				GameObject preloader = (GameObject)GameObject.Instantiate (Resources.Load (AssetConfig.PRELOADER.path), Vector3.zero, Quaternion.identity);
				preloader.name = AssetConfig.PRELOADER.id;
				preloader.transform.SetParent (viewGO.transform, false);

				PreloaderView preloaderView = preloader.GetComponent<PreloaderView> ();
				preloaderView.updateBgTransform(viewGO.GetComponent<RectTransform> ());
			}
		}
	}
}

