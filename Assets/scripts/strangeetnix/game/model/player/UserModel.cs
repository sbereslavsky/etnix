using System;
using UnityEngine;

namespace strangeetnix.game
{
	public class UserModel : IUserModel
	{
		public int id { get; private set; }

		public int hp { get; set; }
		public int startHp { get; private set; }

		public int exp { get; set; }
		public int expStart { get; private set; }
		public int expEnd { get; private set; }

		public int level { get; set; }

		public bool levelUp { get; set; }

		public int char_str { get { return (id == 1) ? _levelDataVO.ch1_str : _levelDataVO.ch2_str; } }
		public int char_dex { get { return (id == 1) ? _levelDataVO.ch1_dex : _levelDataVO.ch2_dex; } }
		public int char_hp { get { return (id == 1) ? _levelDataVO.ch1_hp : _levelDataVO.ch2_hp; } }

		public IUserCharVO userCharVO { get; private set; }

		protected IGameConfig gameConfig { get; private set; }

		private IUserCharVO _userCharVO;

		private ICharAllVO _levelDataVO;

		public UserModel (int setId, IGameConfig gameConfig1)
		{
			id = setId;
			gameConfig = gameConfig1;
			userCharVO = gameConfig.userCharConfig.getUserCharVOById (setId);
			exp = userCharVO.exp;

			levelUp = false;

			setLevelDataVO ();
			setEndExp ();
		}

		public void setLevelDataVO()
		{
			_levelDataVO = gameConfig.charAllConfig.getCharAllVOByExp (exp);

			level = _levelDataVO.level_id;
			expStart = _levelDataVO.exp_next;

			hp = char_hp;
			startHp = hp;
		}

		public void setEndExp()
		{
			Debug.Log ("updateNextExp: level = " + level + ", expStart = expStart");

			ICharAllVO nextLevelDataVO = gameConfig.charAllConfig.getCharAllVOByLevel (level+1);
			if (nextLevelDataVO != null) {
				expEnd = nextLevelDataVO.exp_next;
			} else {
				const int ADD_EXP = 1000;
				expEnd += ADD_EXP;
				Debug.Log ("updateNextExp: nextLevelDataVO = null!");
			}
		}

		public void addHp(int value)
		{
			hp = Mathf.Min (hp + value, startHp);
		}

		public void decHp(int value)
		{
			hp = Mathf.Max (hp - value, 0);
		}

		public void addExp(int value)
		{
			exp += value;

			if (exp >= expEnd) {
				levelUp = true;
			}
		}

		public void resetHp ()
		{
			hp = startHp;
		}
	}
}

