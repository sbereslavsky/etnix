using System;
using UnityEngine;
using strange.extensions.context.impl;
using strangeetnix.game;
using strangeetnix.ui;

namespace strangeetnix.main
{
	public class MainContext : SignalContext
	{
		public MainContext (MonoBehaviour contextView) : base(contextView)
		{
			#if UNITY_ANDROID
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			#endif
		}

		protected override void mapBindings()
		{
			base.mapBindings ();

			if (Context.firstContext == this)
			{
				injectionBinder.Bind<IGameConfig> ().To<GameConfig> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<IGameModel> ().To<GameModel> ().ToSingleton().CrossContext();

				injectionBinder.Bind<IConfigManager> ().To<ConfigManager> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<IResourceManager> ().To<ResourceManager> ().ToSingleton ().CrossContext ();

				injectionBinder.Bind<AddPreloaderSignal> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<DestroyPreloaderSignal> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<UpdatePreloaderValueSignal> ().ToSingleton ().CrossContext();

				injectionBinder.Bind<LoadResourcesSignal> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<LoadDataSignal> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<LoadedDataSignal> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<AddHpSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<GameStartSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<GameInputSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<EnterRoomSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<ExitRoomSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<SwitchLevelSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<CleanCoinsSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<CreateEnemyWaveSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<CreateEnemySpawnerSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<ResetGameCameraSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<GameOverSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<PauseGameSignal> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<RestartGameSignal> ().ToSingleton ().CrossContext ();
				injectionBinder.Bind<LevelStartSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<LevelEndSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<UpdatePlayerInfoSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<UpdateHudItemSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<UpdateGameCanvasSignal> ().ToSingleton ().CrossContext();
				injectionBinder.Bind<DestroyGameFieldSignal> ().ToSingleton ().CrossContext();
			}

			//commandBinder.Bind<SwitchCanvasSignal> ().To<SwitchCanvasCommand> ();
			mediationBinder.Bind<PreloaderCanvasView> ().To<PreloaderCanvasMediator> ();

			commandBinder.Bind <StartSignal> ()
				.To<StartAppCommand> ()
				.Once ().InSequence ();
		}
	}
}