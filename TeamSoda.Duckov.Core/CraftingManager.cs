using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class CraftingManager : MonoBehaviour
{
	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000C97 RID: 3223 RVA: 0x000357E1 File Offset: 0x000339E1
	private static CraftingFormulaCollection FormulaCollection
	{
		get
		{
			return CraftingFormulaCollection.Instance;
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000357E8 File Offset: 0x000339E8
	// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000357EF File Offset: 0x000339EF
	public static CraftingManager Instance { get; private set; }

	// Token: 0x06000C9A RID: 3226 RVA: 0x000357F7 File Offset: 0x000339F7
	private void Awake()
	{
		CraftingManager.Instance = this;
		this.Load();
		SavesSystem.OnCollectSaveData += this.Save;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00035816 File Offset: 0x00033A16
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00035829 File Offset: 0x00033A29
	private void Save()
	{
		SavesSystem.Save<List<string>>("Crafting/UnlockedFormulaIDs", this.unlockedFormulaIDs);
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0003583C File Offset: 0x00033A3C
	private void Load()
	{
		this.unlockedFormulaIDs = SavesSystem.Load<List<string>>("Crafting/UnlockedFormulaIDs");
		if (this.unlockedFormulaIDs == null)
		{
			this.unlockedFormulaIDs = new List<string>();
		}
		foreach (CraftingFormula craftingFormula in CraftingManager.FormulaCollection.Entries)
		{
			if (craftingFormula.unlockByDefault && !this.unlockedFormulaIDs.Contains(craftingFormula.id))
			{
				this.unlockedFormulaIDs.Add(craftingFormula.id);
			}
		}
		this.unlockedFormulaIDs.Sort();
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000C9E RID: 3230 RVA: 0x000358E0 File Offset: 0x00033AE0
	public static IEnumerable<string> UnlockedFormulaIDs
	{
		get
		{
			if (!(CraftingManager.Instance == null))
			{
				foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
				{
					if (CraftingManager.IsFormulaUnlocked(craftingFormula.id))
					{
						yield return craftingFormula.id;
					}
				}
				IEnumerator<CraftingFormula> enumerator = null;
			}
			yield break;
			yield break;
		}
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x000358EC File Offset: 0x00033AEC
	public static void UnlockFormula(string formulaID)
	{
		if (CraftingManager.Instance == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(formulaID))
		{
			Debug.LogError("Invalid formula ID");
			return;
		}
		CraftingFormula craftingFormula = CraftingManager.FormulaCollection.Entries.FirstOrDefault((CraftingFormula e) => e.id == formulaID);
		if (!craftingFormula.IDValid)
		{
			Debug.LogError("Invalid formula ID: " + formulaID);
			return;
		}
		if (craftingFormula.unlockByDefault)
		{
			Debug.LogError("Formula is unlocked by default: " + formulaID);
			return;
		}
		if (CraftingManager.Instance.unlockedFormulaIDs.Contains(formulaID))
		{
			return;
		}
		CraftingManager.Instance.unlockedFormulaIDs.Add(formulaID);
		Action<string> onFormulaUnlocked = CraftingManager.OnFormulaUnlocked;
		if (onFormulaUnlocked == null)
		{
			return;
		}
		onFormulaUnlocked(formulaID);
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x000359C8 File Offset: 0x00033BC8
	private UniTask<List<Item>> Craft(CraftingFormula formula)
	{
		CraftingManager.<Craft>d__17 <Craft>d__;
		<Craft>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Craft>d__.formula = formula;
		<Craft>d__.<>1__state = -1;
		<Craft>d__.<>t__builder.Start<CraftingManager.<Craft>d__17>(ref <Craft>d__);
		return <Craft>d__.<>t__builder.Task;
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x00035A0C File Offset: 0x00033C0C
	public UniTask<List<Item>> Craft(string id)
	{
		CraftingManager.<Craft>d__18 <Craft>d__;
		<Craft>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Craft>d__.<>4__this = this;
		<Craft>d__.id = id;
		<Craft>d__.<>1__state = -1;
		<Craft>d__.<>t__builder.Start<CraftingManager.<Craft>d__18>(ref <Craft>d__);
		return <Craft>d__.<>t__builder.Task;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00035A57 File Offset: 0x00033C57
	internal static bool IsFormulaUnlocked(string value)
	{
		return !(CraftingManager.Instance == null) && !string.IsNullOrEmpty(value) && CraftingManager.Instance.unlockedFormulaIDs.Contains(value);
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00035A84 File Offset: 0x00033C84
	internal static CraftingFormula GetFormula(string id)
	{
		CraftingFormula result;
		if (CraftingFormulaCollection.TryGetFormula(id, out result))
		{
			return result;
		}
		return default(CraftingFormula);
	}

	// Token: 0x04000AFE RID: 2814
	public static Action<CraftingFormula, Item> OnItemCrafted;

	// Token: 0x04000AFF RID: 2815
	public static Action<string> OnFormulaUnlocked;

	// Token: 0x04000B01 RID: 2817
	private const string SaveKey = "Crafting/UnlockedFormulaIDs";

	// Token: 0x04000B02 RID: 2818
	private List<string> unlockedFormulaIDs = new List<string>();
}
