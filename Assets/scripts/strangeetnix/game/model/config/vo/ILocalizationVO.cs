using System;

namespace strangeetnix.game
{
	public interface ILocalizationVO
	{
		string key { get; }
		string en { get; }
		string ru { get; }
	}
}