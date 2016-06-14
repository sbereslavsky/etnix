/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using System;
using UnityEngine;
using strange.extensions.command.impl;

namespace strangeetnix.game
{
	public class RestartGameCommand : Command
	{
		[Inject]
		public GameStartSignal gameStartSignal{get;set;}

		[Inject]
		public LevelEndSignal levelEndSignal{ get; set; }

		[Inject]
		public DestroyGameFieldSignal destroyGameFieldSignal{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }
		
		public override void Execute()
		{
			levelEndSignal.Dispatch ();
			destroyGameFieldSignal.Dispatch ();

			gameModel.playerModel.resetHp ();

			gameStartSignal.Dispatch();
		}
	}
}

