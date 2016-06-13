using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strangeetnix.game;

namespace strangeetnix.main
{
	public class LoadGameDataCommand : Command
	{
		[Inject]
		public IGameConfig gameConfig{get;set;}

		[Inject]
		public IGameModel gameModel{get;set;}

		[Inject]
		public LoadedDataSignal loadedDataSignal{ get; set; }

		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		public override void Execute()
		{
			gameConfig.loadedDataSignal.AddListener (onDataLoaded);
			gameConfig.Load (routineRunner);
			gameModel.Reset ();
		}

		private void onDataLoaded()
		{
			gameConfig.loadedDataSignal.RemoveListener (onDataLoaded);

			loadedDataSignal.Dispatch ();

			Release ();
		}
	}
}
