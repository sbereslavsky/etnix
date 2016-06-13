//Bitwise values for user input.
//In KeyboardInput and OnscreenControlsView, these values are combined
//bitwise to indicate the user's input.

using System;

namespace strangeetnix.game
{
	public class GameInputEvent
	{
		public static int NONE = 0;
		public static int RIGHT = 1;
		public static int LEFT = 2;
		public static int HIT = 4;
		public static int QUIT = 8;
	}
}
