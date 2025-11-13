using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001A9 RID: 425
[CreateAssetMenu]
public class DecomposeDatabase : ScriptableObject
{
	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00035AB9 File Offset: 0x00033CB9
	public static DecomposeDatabase Instance
	{
		get
		{
			return GameplayDataSettings.DecomposeDatabase;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00035AC0 File Offset: 0x00033CC0
	private Dictionary<int, DecomposeFormula> Dic
	{
		get
		{
			if (this._dic == null)
			{
				this.RebuildDictionary();
			}
			return this._dic;
		}
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00035AD8 File Offset: 0x00033CD8
	public void RebuildDictionary()
	{
		this._dic = new Dictionary<int, DecomposeFormula>();
		foreach (DecomposeFormula decomposeFormula in this.entries)
		{
			this._dic[decomposeFormula.item] = decomposeFormula;
		}
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x00035B20 File Offset: 0x00033D20
	public DecomposeFormula GetFormula(int itemTypeID)
	{
		DecomposeFormula result;
		if (!this.Dic.TryGetValue(itemTypeID, out result))
		{
			return default(DecomposeFormula);
		}
		return result;
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00035B48 File Offset: 0x00033D48
	public static UniTask<bool> Decompose(Item item, int count)
	{
		DecomposeDatabase.<Decompose>d__8 <Decompose>d__;
		<Decompose>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Decompose>d__.item = item;
		<Decompose>d__.count = count;
		<Decompose>d__.<>1__state = -1;
		<Decompose>d__.<>t__builder.Start<DecomposeDatabase.<Decompose>d__8>(ref <Decompose>d__);
		return <Decompose>d__.<>t__builder.Task;
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x00035B93 File Offset: 0x00033D93
	public static bool CanDecompose(int itemTypeID)
	{
		return !(DecomposeDatabase.Instance == null) && DecomposeDatabase.Instance.GetFormula(itemTypeID).valid;
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x00035BB4 File Offset: 0x00033DB4
	public static bool CanDecompose(Item item)
	{
		return !(item == null) && DecomposeDatabase.CanDecompose(item.TypeID);
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x00035BCC File Offset: 0x00033DCC
	public static DecomposeFormula GetDecomposeFormula(int itemTypeID)
	{
		if (DecomposeDatabase.Instance == null)
		{
			return default(DecomposeFormula);
		}
		return DecomposeDatabase.Instance.GetFormula(itemTypeID);
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x00035BFB File Offset: 0x00033DFB
	public void SetData(List<DecomposeFormula> formulas)
	{
		this.entries = formulas.ToArray();
	}

	// Token: 0x04000B03 RID: 2819
	[SerializeField]
	private DecomposeFormula[] entries;

	// Token: 0x04000B04 RID: 2820
	private Dictionary<int, DecomposeFormula> _dic;
}
