using System;
using System.Collections;
using UnityEngine;
using strange.extensions.signal.impl;

namespace strangeetnix
{
	[ExecuteInEditMode]
	[AddComponentMenu("NGUI/Interaction/Button Fill")]
	public class UIButtonFill : UIButton
	{
		private float _fillStep;
		private float _fillTime;
		private float _fillValue;

		private string _textValue;

		private UISprite _sprite;

		public UIButtonFill ()
		{
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

			if (this.GetComponentInChildren<UILabel> ().text.Length > 0) {
				_textValue = this.GetComponentInChildren<UILabel> ().text;
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
			this.GetComponentInChildren<UILabel> ().text = (isShow) ? _textValue : "";
		}
	}
}

