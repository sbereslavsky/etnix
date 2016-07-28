using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace strangeetnix.ui
{
	public class Tooltip : MonoBehaviour
	{
		public Text text;

		//if the tooltip is inside a UI element
		bool inside;

		bool xShifted = false;
		bool yShifted = false;

		int textLength;

		float width;
		float height;

		int screenWidth;
		int screenHeight;

		float canvasWidth;
		float canvasHeight;

		float yShift;
		float xShift;

		int canvasMode = 0;

		public Tooltip ()
		{
		}

		public void SetTooltip (string ttext){
			//ScreenSpaceOverlay Tooltip
			//set the text and fit the tooltip panel to the text size
			text.text = ttext;

			this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(text.preferredWidth+60f, text.preferredHeight+20f);
			width = this.transform.GetComponent<RectTransform>().sizeDelta[0];
			height = this.transform.GetComponent<RectTransform>().sizeDelta[1];

			Vector3 newPos = Input.mousePosition-new Vector3(xShift,yShift,0f);
			//check and solve problems for the tooltip that goes out of the screen on the horizontal axis
			float val;
			val=(newPos.x-(width/2));
			if(val<=0){
				newPos.x+=(-val);
			}
			val=(newPos.x+(width/2));
			if(val>screenWidth){
				newPos.x-=(val-screenWidth);
			}
			//check and solve problems for the tooltip that goes out of the screen on the vertical axis
			val=(screenHeight-newPos.y-(height/2));
			if( val<=0 && !yShifted){
				yShift=(-yShift+25f);
				newPos.y+=yShift*2;
				yShifted=true;
			}
			this.transform.position=newPos;
			this.gameObject.SetActive(true);

			inside = true;
			//WorldSpace Tooltip
		}

		public void HideTooltip(){
			//ScreenSpaceOverlay Tooltip
			xShift = 40f;yShift = -30f;
			xShifted=yShifted=false;
			this.transform.position=Input.mousePosition-new Vector3(xShift,yShift,0f);
			this.gameObject.SetActive(false);
			inside = false;
		}

		void FixedUpdate ()
		{
			if (inside) {
				Vector3 newPos = Input.mousePosition - new Vector3 (xShift, yShift, 0f);
				//check and solve problems for the tooltip that goes out of the screen on the horizontal axis
				float val;
				val = (newPos.x - (width / 2));
				if (val <= 0) {
					newPos.x += (-val);
				}
				val = (newPos.x + (width / 2));
				if (val > screenWidth) {
					newPos.x -= (val - screenWidth);
				}
				//check and solve problems for the tooltip that goes out of the screen on the vertical axis
				val = (screenHeight - newPos.y - (height / 2));
				if (val <= 0) {
					if (!yShifted) {
						yShift = (-yShift + 25f);
						newPos.y += yShift * 2;
						yShifted = true;
					}
				}
				this.transform.position = newPos;
			}
		}
	}
}

