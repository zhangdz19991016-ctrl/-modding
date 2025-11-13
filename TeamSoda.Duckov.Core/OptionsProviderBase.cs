using System;
using UnityEngine;

// Token: 0x020001CB RID: 459
public abstract class OptionsProviderBase : MonoBehaviour
{
	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000DB3 RID: 3507
	public abstract string Key { get; }

	// Token: 0x06000DB4 RID: 3508
	public abstract string[] GetOptions();

	// Token: 0x06000DB5 RID: 3509
	public abstract string GetCurrentOption();

	// Token: 0x06000DB6 RID: 3510
	public abstract void Set(int index);
}
