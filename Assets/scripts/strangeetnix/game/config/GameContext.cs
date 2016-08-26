using System;
using System.Collections.Generic;
using strange.extensions.context.impl;
using UnityEngine;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;

namespace strangeetnix.game
{
	public class GameContext : SignalContext
	{
		public GameContext (MonoBehaviour contextView) : base(contextView)
		{
		}
		
		protected override void mapBindings()
		{
			//We need to call mapBindings up the inheritance chain (see SignalContext)
			base.mapBindings ();

			#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)

			//If we're on mobile, we fulfill this as an empty dependency...
			//The job is fulfilled by OnscreenControlsView and its Mediator

			injectionBinder.Bind<IInput> ().To<NullInput> ().ToSingleton ();
			#else

			//But if we're on desktop/editor/web, we map in a Keyboard controller.

			injectionBinder.Bind<IInput> ().To<KeyboardInput> ().ToSingleton ();
			#endif

			//Injection
			if (Context.firstContext == this)
			{

				//In the multi-context world, this dependency is fulfilled Cross-Context by MainContext
				//(that way the same GameModel is shared between all Contexts).
				//When we run standalone, we need to provide it here.
				injectionBinder.Bind<IGameModel> ().To<GameModel> ().ToSingleton ();
				injectionBinder.Bind<IGameConfig> ().To<GameConfig>().ToSingleton ();
			}

			injectionBinder.Bind<ISpawner> ().To<EnemySpawner>().ToSingleton ();

			injectionBinder.Bind<IScoreModel>().To<ScoreModel>().ToSingleton();
			
			mediationBinder.Bind<PlayerView>().To<PlayerMediator>();
			mediationBinder.Bind<EnemyView>().To<EnemyMediator>();
			mediationBinder.Bind<DropCoinView>().To<DropCoinMediator>();

			//Pools
			//Pools provide a recycling system that makes the game much more efficient. Instead of destroying instances
			//(missiles/rocks/enemies/explosions) and re-instantiating them -- which is expensive -- we "checkout" the instances
			//from a pool, then return them when done.

			//These bindings setup the necessary pools, each as a Named injection, so we can tell the pools apart.
			injectionBinder.Bind<IEnemyPoolManager>().To<EnemyPoolManager>().ToSingleton();

			//Signals (not bound to Commands)
			//When a Signal isn't bound to a Command, it needs to be mapped, just like any other injected instance
			injectionBinder.Bind<GameStartedSignal> ().ToSingleton ();
			injectionBinder.Bind<LevelStartedSignal> ().ToSingleton ();
			injectionBinder.Bind<HitEnemySignal> ().ToSingleton ();
			injectionBinder.Bind<HitPlayerSignal> ().ToSingleton ();
			injectionBinder.Bind<StopEnemySignal> ().ToSingleton ();
			injectionBinder.Bind<CameraEnabledSignal> ().ToSingleton ().CrossContext();

			if (Context.firstContext == this)
			{
				//These signals are provided by MainContext when we're in a multi-context situation
				injectionBinder.Bind<GameInputSignal> ().ToSingleton ();
				injectionBinder.Bind<UpdateHudItemSignal> ().ToSingleton ();
			}

			//Commands
			//All Commands get mapped to a Signal that Executes them.
			if (this == Context.firstContext)
			{
				commandBinder.Bind <StartSignal>()
					.To<game.GameIndependentStartCommand> ()
					.Once ().InSequence ();
			}
			else
			{
				commandBinder.Bind <StartSignal>()
					//.To<KillAudioListenerCommand>()
					.To<game.GameModuleStartCommand> ()
					.Once ().InSequence ();
			}

			//All the Signals/Commands necessary to play the game
			//Note:
			//1. Some of these are marked Pooled().
			//   Pooled Commands are more efficient when called repeatedly, but take up memory.
			//   Mark a Command as pooled if it will be called a lot...as in the main game loop.
			//2. Binding a Signal to a Command automatically maps the signal for injection.
			//   So it's only necessary to explicitly injectionBind Signals if they are NOT
			//   mapped to Commands.
			commandBinder.Bind<LoadResourcesSignal> ().To<LoadResourcesCommand> ();

			commandBinder.Bind<AddHpSignal> ().To<AddHpCommand> ();
			commandBinder.Bind<AddExpSignal> ().To<AddExpCommand> ();
			commandBinder.Bind<EnterRoomSignal> ().To<EnterRoomCommand> ();
			commandBinder.Bind<ExitRoomSignal> ().To<ExitRoomCommand> ();
			commandBinder.Bind<SwitchLevelSignal> ().To<SwitchLevelCommand> ();
			commandBinder.Bind<ResetGameCameraSignal> ().To<ResetGameCameraCommand> ();
			commandBinder.Bind<CreateEnemySignal> ().To<CreateEnemyCommand> ().Pooled();
			commandBinder.Bind<CreateEnemyWaveSignal> ().To<CreateEnemyWaveCommand> ().Pooled();
			commandBinder.Bind<CreateEnemySpawnerSignal> ().To<CreateEnemySpawnerCommand> ().Pooled();
			commandBinder.Bind<StopEnemySpawnerSignal> ().To<StopEnemySpawnerCommand> ();
			commandBinder.Bind<CreatePlayerSignal> ().To<CreatePlayerCommand> ();
			commandBinder.Bind<DestroyEnemySignal>().To<DestroyEnemyCommand>().Pooled();
			commandBinder.Bind<DestroyPlayerSignal>().To<DestroyPlayerCommand>().Pooled();
			commandBinder.Bind<GameStartSignal> ().To<StartGameCommand> ();
			commandBinder.Bind<GameOverSignal> ().To<GameOverCommand> ();
			commandBinder.Bind<PauseGameSignal> ().To<PauseGameCommand> ();
			commandBinder.Bind<RestartGameSignal> ().To<RestartGameCommand> ();
			commandBinder.Bind<AddExplosionSignal> ().To<AddExplosionCommand> ();
			commandBinder.Bind<CreateCoinSignal> ().To<CreateCoinsCommand> ();
			commandBinder.Bind<CleanCoinsSignal> ().To<CleanCoinsCommand> ();


			commandBinder.Bind<DestroyGameFieldSignal> ().To<DestroyGameFieldCommand> ();

			//Notice how we can bind ONE Signal to SEVERAL Commands
			//This allows us to call Commands in sequence...ensuring that the second
			//Command only fires AFTER the first one has completed. This is especially
			//Useful for asynchronous calls, such as server communication.
			commandBinder.Bind<LevelStartSignal> ()
				.To<CreateGameFieldCommand>()
				.To<CleanupLevelCommand>()
				.To<StartLevelCommand> ()
				.InSequence();
			commandBinder.Bind<LevelEndSignal> ()
				.To<CleanupLevelCommand>()
				.To<LevelEndCommand> ()
				.InSequence();
			//commandBinder.Bind<MissileHitSignal> ().To<MissileHitCommand> ().Pooled();
			commandBinder.Bind<SetupLevelSignal> ()
				.To<SetupLevelCommand> ();


			//Mediation
			//Mediation allows us to separate the View code from the rest of the app.
			//The details of **why** mediation is a good thing can be read in the faq:
			//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator
			mediationBinder.Bind<GameFieldView> ().To<GameFieldMediator> ();
			mediationBinder.Bind<GameBackgroundView> ().To<GameBackgroundMediator> ();
			mediationBinder.Bind<GameCameraView> ().To<GameCameraMediator> ();
			mediationBinder.Bind<GameDebugView> ().To<GameDebugMediator> ();
		}

		//After bindings are done, you sometimes want to do more stuff to configure your app.
		//Do that sort of stuff here.
		protected override void postBindings ()
		{
			//Establish our camera. We do this early since it gets injected in places that help us do layout.
			Camera cam = (contextView as GameObject).GetComponentInChildren<Camera> ();
			if (cam == null)
			{
				throw new Exception ("GameContext couldn't find the game camera");
			}
			injectionBinder.Bind<Camera> ().ToValue (cam).ToName (StrangeEtnixElement.GAME_CAMERA);

			// Configure the pools.
			// (Hint: all our pools for this game are identical, but for the content of the InstanceProvider)
			//Strange Pools by default "inflate" as necessary. This means that if you only
			//have one instance in the pool and require another, the pool will instantiate a second.
			//If you have two instances and need another, the pool will inflate again.
			//So long as you keep feeding instances back to the pool, there will always be enough instances,
			//and you'll never create more than you require.
			//The pool can use one of two inflation strategies:
			//1. DOUBLE (the default) is useful when your pool consists of objects that just exist in memory.
			//2. INCREMENT is better for onscreen objects as we're doing here. By INCREMENT-inflating, you'll
			//   get one new GameObject whenever you need one. This minimizes the necessary management of
			//   Views whenever the pool inflates.

			IGameConfig gameConfig = (IGameConfig)injectionBinder.GetInstance<IGameConfig>();
			IEnemyPoolManager enemyPoolManager = (IEnemyPoolManager)injectionBinder.GetInstance<IEnemyPoolManager>();
			if (gameConfig != null && enemyPoolManager != null) {
				IAssetConfig assectConfig = gameConfig.assetConfig;
				foreach (KeyValuePair<int, ICharAssetVO> asset in assectConfig.enemyAssetList) {
					if (asset.Value != null) {
						enemyPoolManager.addPool (asset.Value);
					}
				}
			}

			//Don't forget to call the base version...important stuff happens there!!!
			base.postBindings ();

			if (Context.firstContext != this)
			{
				//Disable the AudioListener
				AudioListener listener = (contextView as GameObject).GetComponentInChildren<AudioListener> ();
				listener.enabled = false;
			}
		}
	}
}
