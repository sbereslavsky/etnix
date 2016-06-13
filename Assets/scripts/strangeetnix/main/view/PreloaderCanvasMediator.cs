using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strangeetnix.ui;
using UnityEngine.SceneManagement;

namespace strangeetnix.main
{
	public class PreloaderCanvasMediator : Mediator
	{
		[Inject]
		public PreloaderCanvasView view { get; set;}

		[Inject]
		public SwitchCanvasSignal switchCanvasSignal  { get; set;}

		public override void OnRegister()
		{
			StartCoroutine(LoadScene());
		}

		public override void OnRemove()
		{
			
		}

		IEnumerator LoadScene() 
		{
			yield return new WaitForSeconds(2f);
			AsyncOperation uiAsync = SceneManager.LoadSceneAsync ("ui", LoadSceneMode.Additive);
			//uiAsync.allowSceneActivation = false;
			while (!uiAsync.isDone) {
				yield return null;
				view.updateText (uiAsync.progress);
			}
			yield return new WaitForSeconds(1f);

			AsyncOperation gameAsync = SceneManager.LoadSceneAsync ("game", LoadSceneMode.Additive);
			//gameAsync.allowSceneActivation = false;
			while (!gameAsync.isDone) {
				view.updateText (gameAsync.progress);
				yield return null;
			}

			//
			Destroy (view.gameObject);
			switchCanvasSignal.Dispatch (UIStates.MAIN);
		}
	}
}

