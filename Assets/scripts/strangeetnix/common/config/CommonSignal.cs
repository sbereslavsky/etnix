//This file contains all the signals that are dispatched between Contexts

using System;
using strange.extensions.signal.impl;
using UnityEngine;

namespace strangeetnix
{
	public class StartSignal : Signal{}
	public class LoadDataSignal : Signal{}
	public class LoadedDataSignal : Signal{}

	//Input
	public class GameInputSignal : Signal<int>{};

	//Game
	//PreloaderTypes - preloader type 
	public class LoadResourcesSignal : Signal<PreloaderTypes>{}

	public class GameStartSignal : Signal{}
	public class GameOverSignal : Signal{}
	//public class GameEndSignal : Signal{}
	public class LevelStartSignal : Signal{}
	public class LevelEndSignal : Signal{}
	public class PauseGameSignal : Signal<bool>{}
	public class RestartGameSignal : Signal{}

	public class UpdateHudItemSignal : Signal<UpdateHudItemType, int>{}
	public class UpdateGameCanvasSignal : Signal{}

	public class UpdatePreloaderValueSignal : Signal<int>{}

	//int - add value
	//bool - is positive
	public class AddHpSignal : Signal<int, bool>{}

	public class DestroyGameFieldSignal : Signal{}
}