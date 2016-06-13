using System;

namespace strangeetnix.game
{
	public class UserCharInfoVO : IUserCharInfoVO
	{
		public int id { get; set;} 
		public string name { get; set;} 
		public int level { get; set;} 
		public int exp { get; set;}
		public int hp { get; set;}
		public string equiped { get; set;}
		public string weapon { get; set;}
		public string item2 { get; set;}
		public string item3 { get; set;}
		//public string wave { get; set;}

		public UserCharInfoVO ()
		{
		}
	}
}