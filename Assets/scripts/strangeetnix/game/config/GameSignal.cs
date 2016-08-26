//All the Signals exclusive to the GameContext

using System;
using strange.extensions.signal.impl;
using UnityEngine;

namespace strangeetnix.game
{
	//Game
	public class GameStartedSignal : Signal{}

	//Player
	//float - x position
	public class CreatePlayerSignal : Signal<float>{}

	//ShipView - reference to the Player
	//float - delay to destroy
	//bool - False indicates destruction. True indicates cleanup at end of level.
	public class DestroyPlayerSignal : Signal<PlayerView, float, bool>{}

	//int - wave id
	public class CreateEnemyWaveSignal : Signal<int>{}
	public class CreateEnemySpawnerSignal : Signal{}
	public class StopEnemySpawnerSignal : Signal{}

	//bool - is camera enabled
	public class CameraEnabledSignal : Signal<bool>{}

	//int - id of the enemy
	//float - position X of the enemy
	public class CreateEnemySignal : Signal<int, float>{}

	//EnemyView - reference to the specific ship
	//float - delay before destroy
	//bool - True indicates player gets points. False is simple cleanup.
	public class DestroyEnemySignal : Signal<EnemyView, float, bool>{}

	//string - enemy name
	//int - dec hp
	public class HitEnemySignal : Signal<EnemyView, int>{}

	//Transform - transform of enemy game object
	//int - dec hp
	public class HitPlayerSignal : Signal<Transform, int>{}

	public class StopEnemySignal : Signal{}

	//Vector2 - explosion position 
	public class AddExplosionSignal : Signal<Vector2>{}

	//Vector2 - coin position 
	//int - coin value
	public class CreateCoinSignal : Signal<Vector2, int>{}
	public class CleanCoinsSignal : Signal{}

	//int - add value
	//public class AddExpSignal : Signal<int>{}

	//GameObject - The GameObject that fired the missile
	//GameElemet - ID to indicate if it is a Player or Enemy missile
	//public class FireMissileSignal : Signal<GameObject, GameElement>{}

	//MissileView - reference to the specific missile
	//GameObject - The contact with which the missile collided
	//public class MissileHitSignal : Signal<MissileView, GameObject>{}

	//Level
	public class SetupLevelSignal : Signal{}
	public class LevelStartedSignal : Signal{}

	//int - room number
	public class SwitchLevelSignal : Signal<int>{}

	//int - wave id
	public class EnterRoomSignal : Signal<int>{}
	public class ExitRoomSignal : Signal{}

	//bool - disabled camera
	public class ResetGameCameraSignal : Signal<bool>{}
}

