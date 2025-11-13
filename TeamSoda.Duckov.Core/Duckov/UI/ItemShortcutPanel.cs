using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B4 RID: 948
	public class ItemShortcutPanel : MonoBehaviour
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x000777B4 File Offset: 0x000759B4
		// (set) Token: 0x0600222D RID: 8749 RVA: 0x000777BC File Offset: 0x000759BC
		public Inventory Target { get; private set; }

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x000777C5 File Offset: 0x000759C5
		// (set) Token: 0x0600222F RID: 8751 RVA: 0x000777CD File Offset: 0x000759CD
		public CharacterMainControl Character { get; internal set; }

		// Token: 0x06002230 RID: 8752 RVA: 0x000777D6 File Offset: 0x000759D6
		private void Awake()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.Initialize();
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000777F6 File Offset: 0x000759F6
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00077809 File Offset: 0x00075A09
		private void OnLevelInitialized()
		{
			this.Initialize();
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x00077814 File Offset: 0x00075A14
		private void Initialize()
		{
			LevelManager instance = LevelManager.Instance;
			this.Character = ((instance != null) ? instance.MainCharacter : null);
			if (this.Character == null)
			{
				return;
			}
			LevelManager instance2 = LevelManager.Instance;
			Inventory target;
			if (instance2 == null)
			{
				target = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance2.MainCharacter;
				if (mainCharacter == null)
				{
					target = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					target = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			this.Target = target;
			if (this.Target == null)
			{
				return;
			}
			for (int i = 0; i < this.buttons.Length; i++)
			{
				ItemShortcutButton itemShortcutButton = this.buttons[i];
				if (!(itemShortcutButton == null))
				{
					itemShortcutButton.Initialize(this, i);
				}
			}
			this.initialized = true;
		}

		// Token: 0x0400170C RID: 5900
		[SerializeField]
		private ItemShortcutButton[] buttons;

		// Token: 0x0400170F RID: 5903
		private bool initialized;
	}
}
