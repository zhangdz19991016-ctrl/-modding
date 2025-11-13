using System;
using System.Collections.Generic;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x0200031A RID: 794
	public class BuildingEffect : MonoBehaviour
	{
		// Token: 0x06001A5D RID: 6749 RVA: 0x0005F86A File Offset: 0x0005DA6A
		private void Awake()
		{
			BuildingManager.OnBuildingListChanged += this.OnBuildingStatusChanged;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0005F88E File Offset: 0x0005DA8E
		private void OnDestroy()
		{
			this.DisableEffects();
			BuildingManager.OnBuildingListChanged -= this.OnBuildingStatusChanged;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0005F8B8 File Offset: 0x0005DAB8
		private void OnLevelInitialized()
		{
			this.Refresh();
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0005F8C0 File Offset: 0x0005DAC0
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0005F8C8 File Offset: 0x0005DAC8
		private void OnBuildingStatusChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0005F8D0 File Offset: 0x0005DAD0
		private void Refresh()
		{
			this.DisableEffects();
			if (this.IsBuildingConstructed())
			{
				this.EnableEffects();
			}
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0005F8E6 File Offset: 0x0005DAE6
		private bool IsBuildingConstructed()
		{
			return BuildingManager.Any(this.buildingID, false);
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x0005F8F4 File Offset: 0x0005DAF4
		private void DisableEffects()
		{
			foreach (Modifier modifier in this.modifiers)
			{
				if (modifier != null)
				{
					modifier.RemoveFromTarget();
				}
			}
			this.modifiers.Clear();
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0005F954 File Offset: 0x0005DB54
		private void EnableEffects()
		{
			this.DisableEffects();
			if (CharacterMainControl.Main == null)
			{
				return;
			}
			foreach (BuildingEffect.ModifierDescription description in this.modifierDescriptions)
			{
				this.Apply(description);
			}
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0005F9BC File Offset: 0x0005DBBC
		private void Apply(BuildingEffect.ModifierDescription description)
		{
			CharacterMainControl main = CharacterMainControl.Main;
			Stat stat;
			if (main == null)
			{
				stat = null;
			}
			else
			{
				Item characterItem = main.CharacterItem;
				stat = ((characterItem != null) ? characterItem.GetStat(description.stat) : null);
			}
			Stat stat2 = stat;
			if (stat2 == null)
			{
				return;
			}
			Modifier modifier = new Modifier(description.type, description.value, this);
			stat2.AddModifier(modifier);
			this.modifiers.Add(modifier);
		}

		// Token: 0x040012ED RID: 4845
		[SerializeField]
		private string buildingID;

		// Token: 0x040012EE RID: 4846
		[SerializeField]
		private List<BuildingEffect.ModifierDescription> modifierDescriptions = new List<BuildingEffect.ModifierDescription>();

		// Token: 0x040012EF RID: 4847
		private List<Modifier> modifiers = new List<Modifier>();

		// Token: 0x020005B2 RID: 1458
		[Serializable]
		public struct ModifierDescription
		{
			// Token: 0x04002084 RID: 8324
			public string stat;

			// Token: 0x04002085 RID: 8325
			public ModifierType type;

			// Token: 0x04002086 RID: 8326
			public float value;
		}
	}
}
