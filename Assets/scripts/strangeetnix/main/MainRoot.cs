using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace strangeetnix.main
{
	public class MainRoot : ContextView
	{
		void Start()
		{
			//Instantiate the context, passing it this instance.
			context = new MainContext(this);

			//add remote logger for Android/IOS
			#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
			GameObject remoteLogger = (GameObject)Instantiate (new GameObject (), Vector3.zero, Quaternion.identity);
			remoteLogger.AddComponent<RemoteLogger> ();
			#endif
		}
	}
}