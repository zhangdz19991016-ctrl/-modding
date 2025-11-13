using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
public static class ColorExtensions
{
	// Token: 0x06000534 RID: 1332 RVA: 0x0001790C File Offset: 0x00015B0C
	public static string ToHexString(this Color color)
	{
		return ((byte)(color.r * 255f)).ToString("X2") + ((byte)(color.g * 255f)).ToString("X2") + ((byte)(color.b * 255f)).ToString("X2") + ((byte)(color.a * 255f)).ToString("X2");
	}
}
