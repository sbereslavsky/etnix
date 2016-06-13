/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

/// Kicks off the app, directly after context binding

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using strangeetnix.ui;
using UnityEngine.SceneManagement;

namespace strangeetnix.main
{
	public class StartAppCommand : Command
	{
		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView { get; set; }

		//private string PRELOADER_CANVAS = "PreloaderCanvas";
		//private string FOLDER_UI   = "ui/";
	
		public override void Execute()
		{
			/*GameObject canvasStyle = Resources.Load<GameObject> (FOLDER_UI+PRELOADER_CANVAS);
			GameObject preloaderGO = GameObject.Instantiate (canvasStyle) as GameObject;
			preloaderGO.name = PRELOADER_CANVAS;
			preloaderGO.transform.parent = contextView.transform;*/
			SceneManager.LoadSceneAsync ("ui", LoadSceneMode.Additive);
			SceneManager.LoadSceneAsync ("game", LoadSceneMode.Additive);
			Release ();
		}
	}
}

