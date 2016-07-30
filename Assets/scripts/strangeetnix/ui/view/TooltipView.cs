using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace strangeetnix.ui
{
	public class TooltipView : MonoBehaviour
	{
		public bool IsActive {
			get {
				return gameObject.activeSelf;
			}
		}
		//public CanvasGroup tooltip;
		public UnityEngine.UI.Text tooltipText;

		void Awake() {
			instance = this;
			HideTooltip();
		}

		public void ShowTooltip(string text, Vector3 pos) {
			if (tooltipText.text != text)
				tooltipText.text = text;

			transform.position = pos;

			gameObject.SetActive(true);
		}

		public void HideTooltip() {
			gameObject.SetActive(false);
		}

		// Standard Singleton Access 
		private static TooltipView instance;
		public static TooltipView Instance
		{
			get
			{
				if (instance == null)					
					instance = GameObject.FindObjectOfType<TooltipView>();
				return instance;
			}
		}
	}
}

