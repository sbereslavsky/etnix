using System;

namespace strangeetnix.game
{
	public interface IUserCharInfoVO
	{
		int id { get; set; }
		string name { get; set; } 
		int level { get; set;} 
		int hp { get; set; }
		int exp { get; set; }
		string equiped { get; set; }
		string weapon { get; set; }
		string item2 { get; set; }
		string item3 { get; set; }
		//string wave { get; set; }
	}
}
