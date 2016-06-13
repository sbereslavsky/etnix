using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class AddExplosionCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		[Inject]
		public Vector2 position { get; set; }

		public override void Execute ()
		{
			GameObject explosionStyle = Resources.Load<GameObject> ("fx/explosion");
			GameObject explosionGO = GameObject.Instantiate (explosionStyle) as GameObject;
			explosionGO.transform.localPosition = new Vector3(position.x, position.y, 0);
			explosionGO.transform.parent = gameField.transform;
			GameObject.Destroy (explosionGO, 0.3f);
		}
	}
}

