﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace strangeetnix.ui
{
	public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		public string text;

		public void OnPointerEnter(PointerEventData eventData)
		{
			StartHover(new Vector3(eventData.position.x, eventData.position.y - 18f, 0f));
		}   
		public void OnSelect(BaseEventData eventData)
		{
			StartHover(transform.position);
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			StopHover();
		}
		public void OnDeselect(BaseEventData eventData)
		{
			StopHover();
		}

		void StartHover(Vector3 position) {
			TooltipView.Instance.ShowTooltip(text, position);
		}
		void StopHover() {
			TooltipView.Instance.HideTooltip();
		}
	}
}

