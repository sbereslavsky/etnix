﻿//Interface for a service.
//In this case, the service allows us to run a Coroutine outside
//the strict confines of a MonoBehaviour. (See RoutineRunner)

using System;
using UnityEngine;
using System.Collections;

namespace strangeetnix
{
	public interface IRoutineRunner
	{
		Coroutine StartCoroutine(IEnumerator method);
		void StopCoroutine (IEnumerator method);
	}
}