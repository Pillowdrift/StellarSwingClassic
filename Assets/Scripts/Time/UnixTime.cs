using UnityEngine;
using System;
using System.Collections;

public class UnixTime
{
	public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static string Current
	{
		get
		{
			return (DateTime.Now.ToUniversalTime() - Epoch).TotalSeconds.ToString();
		}
	}
}
