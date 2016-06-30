using System;

namespace strangeetnix.game
{
	public interface ICharInfoVO
	{
		int id { get;}
		string name { get;}
		float speed { get;}
		int moveForce { get;}
	}
}