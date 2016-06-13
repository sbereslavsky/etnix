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
		}
	}
}