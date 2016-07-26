using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using strange.extensions.signal.impl;

namespace strangeetnix
{
	[ExecuteInEditMode]
	[AddComponentMenu("NGUI/Interaction/Button Pointer")]
	public class UIButtonPointer : UIButton
	{
		internal Signal pointerDownSignal = new Signal(); 
		internal Signal pointerUpSignal = new Signal();

		private float _fillStep;
		private float _fillTime;
		private float _fillValue;

		private string _textValue;

		private UISprite _sprite;

		private bool _isPressed = false;

		public UIButtonPointer ()
		{
			_isPressed = false;
			_sprite = GetComponent<UISprite> ();
		}

		public bool isFilled {get { return _sprite.fillAmount == 1;}}

		public void startFill(float fillTime, float fillStep)
		{
			_fillTime = fillTime;
			_fillStep = fillStep;
			_fillValue = 0;

			if (_sprite.fillAmount < 1) {
				StopCoroutine (decreaseTimeRemaining());
			}

			if (this.GetComponentInChildren<Text> ().text.Length > 0) {
				_textValue = this.GetComponentInChildren<Text> ().text;
			}

			setTextValue (false);

			_sprite.fillAmount = 0;
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
				_sprite.fillAmount = _fillValue;

				if (_fillValue >= 1) {
					setTextValue(true);
				}
			}
		}

		private void setTextValue(bool isShow)
		{
			this.GetComponentInChildren<Text> ().text = (isShow) ? _textValue : "";
		}

		protected override void OnPress (bool isPressed)
		{
			base.OnPress (isPressed);

			if (!_isPressed && isPressed) {
				pointerDownSignal.Dispatch ();
			} else if (_isPressed && !isPressed) {
				pointerUpSignal.Dispatch ();
			}
			_isPressed = isPressed;
		}
	}
}

