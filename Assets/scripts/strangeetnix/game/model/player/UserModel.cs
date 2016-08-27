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

		virtual public int char_str { get { return _char_str; } }
		virtual public int char_dex { get { return _char_dex; } }
		virtual public int char_hp { get { return _char_hp; } }

		public IUserCharVO userCharVO { get; private set; }

		private IUserCharVO _userCharVO;
		private ICharAllVO _levelDataVO;

		protected IGameConfig gameConfig { get; private set; }

		protected int _char_str = 0;
		protected int _char_dex = 0;
		protected int _char_hp = 0;

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
			if (id == 1) {
				_char_str = _levelDataVO.ch1_str;
				_char_dex = _levelDataVO.ch1_dex;
				_char_hp = _levelDataVO.ch1_hp;
			} else {
				_char_str = _levelDataVO.ch2_str;
				_char_dex = _levelDataVO.ch2_dex;
				_char_hp = _levelDataVO.ch2_hp;
			}

			level = _levelDataVO.level_id;
			expStart = _levelDataVO.exp_next;

			updateHp();

			setEndExp ();

			levelUp = false;
		}

		public void updateHp()
		{
			hp = char_hp;
			setStartHp (hp);
		}

		protected void setStartHp(int value)
		{
			startHp = value;
		}

		private void setEndExp()
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

