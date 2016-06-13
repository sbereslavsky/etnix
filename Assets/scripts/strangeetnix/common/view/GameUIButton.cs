using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using strange.extensions.signal.impl;

namespace strangeetnix
{
	public class GameUIButton : Button
	{
		internal Signal pointerDownSignal = new Signal(); 
		internal Signal pointerUpSignal = new Signal();

		private float _fillStep;
		private float _fillTime;
		private float _fillValue;

		private string _textValue;

		public GameUIButton ()
		{
			
		}

		public bool isFilled {get { return image.fillAmount == 1;}}

		public void startFill(float fillTime, float fillStep)
		{
			_fillTime = fillTime;
			_fillStep = fillStep;
			_fillValue = 0;

			if (image.fillAmount < 1) {
				StopCoroutine (decreaseTimeRemaining());
			}

			if (this.GetComponentInChildren<Text> ().text.Length > 0) {
				_textValue = this.GetComponentInChildren<Text> ().text;
			}

			setTextValue (false);

			image.fillAmount = 0;
			StartCoroutine (decreaseTimeRemaining());
		}

		public void restart()
		{
			if (_fillValue < 1) {
				_fillValue = 1;
			}

			if (_textValue != null) {
				setTextValue (true);
			}
		}

		IEnumerator decreaseTimeRemaining()
		{
			while (_fillValue < 1) {
				yield return new WaitForSeconds (_fillStep);
				_fillValue += _fillStep / _fillTime;
				image.fillAmount = _fillValue;

				if (_fillValue >= 1) {
					setTextValue(true);
				}
			}
		}

		private void setTextValue(bool isShow)
		{
			this.GetComponentInChildren<Text> ().text = (isShow) ? _textValue : "";
		}

		public override void OnPointerDown (PointerEventData eventData)
		{
			pointerDownSignal.Dispatch ();
		}

		public override void OnPointerUp (PointerEventData eventData)
		{
			pointerUpSignal.Dispatch ();
		}
	}
}

