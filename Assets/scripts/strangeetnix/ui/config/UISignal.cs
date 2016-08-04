//All the Signals exclusive to the GameContext

using System;
using strange.extensions.signal.impl;
using UnityEngine;

namespace strangeetnix.ui
{
	//int - The char id
	public class ChoosePlayerSignal : Signal<int>{}

	//int - The panel item id
	//int - The char id
	public class AddCharPanelSignal : Signal<int, int>{}

	//int - The char id
	public class RemoveCharPanelSignal : Signal<int>{}

	//int - The char id
	public class EditCharDataSignal : Signal<int>{}

	public class CloseEditPanelSignal : Signal{}

	//UIStates - state canvas
	public class SwitchCanvasSignal : Signal<UIStates>{}

	public class AddDialogSignal : Signal<DialogType>{}

	public class CloseDialogSignal : Signal{}

	//bool - show/hide value
	//int - number of room
	public class ShowRoomButtonSignal : Signal<bool, int>{}
}

