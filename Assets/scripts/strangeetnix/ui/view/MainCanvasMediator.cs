using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class MainCanvasMediator : Mediator
	{
		[Inject]
		public MainCanvasView view { get; set;}

		[Inject]
		public IGameConfig gameConfig {get;set;}

		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public LoadDataSignal loadDataSignal { get; set; }

		[Inject]
		public LoadedDataSignal loadedDataSignal { get; set; }

		[Inject]
		public GameStartSignal gameStartSignal { get; set; }

		[Inject]
		public SwitchCanvasSignal switchCanvasSignal { get; set; }

		//Main canvas signals
		[Inject]
		public AddCharPanelSignal addCharPanelSignal { get; set; }

		[Inject]
		public RemoveCharPanelSignal removeCharPanelSignal { get; set; }

		[Inject]
		public EditCharDataSignal editCharDataSignal { get; set; }

		[Inject]
		public CloseEditPanelSignal closeEditPanelSignal { get; set; }

		[Inject]
		public ChoosePlayerSignal choosePlayerSignal { get; set; }

		[Inject]
		public LoadResourcesSignal loadResourcesSignal { get; set; }

		[Inject]
		public GameInputSignal gameInputSignal{ get; set; }

		private const int MAX_ITEMS_COUNT = 3;

		public override void OnRegister()
		{
			UpdateListeners(true);

			//disable show free dialogs
			/*for (int i = 1; i <= MAX_ITEMS_COUNT; i++) {
				initDialogCharList (i);
			}*/

			if (gameConfig.userCharConfig == null) {
				loadDataSignal.Dispatch ();
			} else {
				onLoadedGameData ();
			}
		}

		public override void OnRemove()
		{
			UpdateListeners(false);
			destroyPanels ();
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				loadedDataSignal.AddListener (onLoadedGameData);
				addCharPanelSignal.AddListener (onAddItem);
				editCharDataSignal.AddListener (onEditItem);
				removeCharPanelSignal.AddListener (onRemoveItem);
				closeEditPanelSignal.AddListener (onCloseDialogCharEdit);
				choosePlayerSignal.AddListener (onChoosePlayer);

				gameInputSignal.AddListener (onGameInput);
			} else {
				loadedDataSignal.RemoveListener (onLoadedGameData);
				addCharPanelSignal.RemoveListener (onAddItem);
				editCharDataSignal.RemoveListener (onEditItem);
				removeCharPanelSignal.RemoveListener (onRemoveItem);
				closeEditPanelSignal.RemoveListener (onCloseDialogCharEdit);
				choosePlayerSignal.RemoveListener (onChoosePlayer);

				gameInputSignal.RemoveListener (onGameInput);
			}
		}

		//Receive a signal updating GameInput
		private void onGameInput(int input)
		{
			if ((input & GameInputEvent.QUIT) > 0) {
				Application.Quit ();
			}
		}

		private void onLoadedGameData()
		{
			destroyPanels ();
			for (byte i = 0; i < gameConfig.userCharConfig.list.Count; i++) {
				IUserCharVO userCharVO = gameConfig.userCharConfig.list[i];
				if (userCharVO.classId > 0 && i < MAX_ITEMS_COUNT) {
					initDialogCharInfo (i + 1, userCharVO);
				} else {
					initDialogCharList (i + 1);
				}
			}
		}

		private void onAddItem(int dialogId, int charId)
		{
			IUserCharVO userCharVO = (gameConfig != null) ? gameConfig.userCharConfig.getUserCharVOById(charId) : null;
			initDialogCharInfo (dialogId, userCharVO);
		}

		private void onRemoveItem(int dialogId)
		{
			view.destroyPanelByIndex(dialogId);
			initDialogCharList (dialogId);
		}

		private void onEditItem(int charId)
		{
			initDialogCharEdit (charId);
		}

		private void destroyPanels()
		{
			for (int i = 1; i <= MAX_ITEMS_COUNT; i++) {
				view.destroyPanelByIndex (i);
			}
		}

		private void onChoosePlayer(int playerId)
		{
			destroyPanels ();
			gameModel.playerId = playerId;
			gameModel.initLevelData (gameConfig);

			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			gameStartSignal.Dispatch ();
			switchCanvasSignal.Dispatch (UIStates.GAME);
			#elif UNITY_ANDROID || UNITY_IPHONE
			loadResourcesSignal.Dispatch(PreloaderTypes.MAIN);
			#endif
		}

		private void onCloseDialogCharEdit()
		{
			view.destroyDialogCharEdit ();
			onLoadedGameData ();
		}

		private void initDialogCharEdit(int charId)
		{
			GameObject userDialog = view.getUserDialog (DialogType.CHAR_EDIT, charId);

			DialogCharEditView DialogCharEditView = userDialog.GetComponent<DialogCharEditView> ();
			if (DialogCharEditView) {
				DialogCharEditView.charId = charId;
			}

			destroyPanels ();
		}

		private void initDialogCharInfo(int dialogId, IUserCharVO userCharVO)
		{
			GameObject userDialog = view.getUserDialog (DialogType.CHAR_INFO, dialogId);

			DialogCharInfoView charInfoView = userDialog.GetComponent<DialogCharInfoView> ();
			if (charInfoView) {
				IUserCharInfoVO userCharInfoVO = userCharVO.getUserCharInfoVO (gameConfig);
				IPlayerModel playerModel = new PlayerModel (userCharVO.id, gameConfig);
				charInfoView.init (userCharInfoVO, playerModel, dialogId);
			}
		}

		private void initDialogCharList(int dialogId)
		{
			GameObject panel = view.getUserDialog (DialogType.CHAR_LIST, dialogId);
			if (gameConfig.charInfoConfig != null) {
				DialogCharListView addCharView = panel.GetComponent<DialogCharListView> ();
				if (addCharView) {
					addCharView.init (gameConfig.charInfoConfig.names, dialogId);
				}
			}
		}
	}
}

