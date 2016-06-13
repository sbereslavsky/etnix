﻿//Mediates the ButtonView in Editor/Desktop/Web...wherever mouse clicks are appropriate

using System;
using strange.extensions.mediation.impl;

namespace strangeetnix
{
	public class ButtonMouseMediator : Mediator
	{
		[Inject]
		public ButtonView view { get; set; }

		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

		protected void OnMouseDown()
		{
			view.pressBegan ();
		}

		protected void OnMouseUp()
		{
			view.pressEnded ();
		}

		protected void OnMouseEnter()
		{
			view.background.GetComponent<UnityEngine.Renderer>().material.color = view.overColor;
		}

		protected void OnMouseExit()
		{
			view.background.GetComponent<UnityEngine.Renderer>().material.color = view.normalColor;
		}

		#endif
	}
}

