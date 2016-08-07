using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strangeetnix.game;
using strangeetnix.ui;

namespace strangeetnix.main
{
	public class LoadGameDataCommand : Command
	{
		[Inject]
		public IGameConfig gameConfig{get;set;}

		[Inject]
		public IGameModel gameModel{get;set;}

		[Inject]
		public IConfigManager configManager{get;set;}

		[Inject]
		public LoadedDataSignal loadedDataSignal{ get; set; }

		[Inject]
		public AddPreloaderSignal addPreloaderSignal{ get; set; }

		public override void Execute()
		{
			gameConfig.loadedDataSignal.AddListener (onDataLoaded);

			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			gameConfig.loadLocalConfigs ();
			#elif UNITY_ANDROID || UNITY_IPHONE
			addPreloaderSignal.Dispatch(PreloaderTypes.MAIN);
			configManager.startLoad();
			#endif

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
