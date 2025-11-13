using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000238 RID: 568
	public class HardwareSyncingManager : MonoBehaviour
	{
		// Token: 0x1400007A RID: 122
		// (add) Token: 0x060011CF RID: 4559 RVA: 0x000453A0 File Offset: 0x000435A0
		// (remove) Token: 0x060011D0 RID: 4560 RVA: 0x000453D4 File Offset: 0x000435D4
		public static event Action<string> OnSetEvent;

		// Token: 0x060011D1 RID: 4561 RVA: 0x00045408 File Offset: 0x00043608
		public static void SetEvent(string eventName)
		{
			try
			{
				Action<string> onSetEvent = HardwareSyncingManager.OnSetEvent;
				if (onSetEvent != null)
				{
					onSetEvent(eventName);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00045440 File Offset: 0x00043640
		private void Awake()
		{
			InteractableLootbox.OnStartLoot += this.OnStartLoot;
			Health.OnHurt += this.OnHurt;
			Health.OnDead += this.OnDead;
			MainMenu.OnMainMenuAwake = (Action)Delegate.Combine(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Combine(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			LevelManager.OnEvacuated += this.OnEvacuated;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x000454D4 File Offset: 0x000436D4
		private void OnDestroy()
		{
			InteractableLootbox.OnStartLoot -= this.OnStartLoot;
			Health.OnHurt -= this.OnHurt;
			Health.OnDead -= this.OnDead;
			MainMenu.OnMainMenuAwake = (Action)Delegate.Remove(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Remove(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			LevelManager.OnEvacuated -= this.OnEvacuated;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00045565 File Offset: 0x00043765
		private void OnEvacuated(EvacuationInfo info)
		{
			HardwareSyncingManager.SetEvent("Escaped");
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00045571 File Offset: 0x00043771
		private void OnMainMenuAwake()
		{
			HardwareSyncingManager.SetEvent("MainMenu_On");
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004557D File Offset: 0x0004377D
		private void OnMainMenuDestroy()
		{
			HardwareSyncingManager.SetEvent("MainMenu_Off");
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0004558C File Offset: 0x0004378C
		private void OnDead(Health health, DamageInfo info)
		{
			if (health == null)
			{
				return;
			}
			if (health.IsMainCharacterHealth)
			{
				HardwareSyncingManager.SetEvent("Die");
				return;
			}
			if (info.fromCharacter != null && info.fromCharacter.IsMainCharacter)
			{
				HardwareSyncingManager.SetEvent("Kill_Enemy");
			}
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000455DB File Offset: 0x000437DB
		private void OnHurt(Health health, DamageInfo info)
		{
			if (health == null)
			{
				return;
			}
			if (health.IsMainCharacterHealth)
			{
				HardwareSyncingManager.SetEvent("Take_Damage");
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000455F9 File Offset: 0x000437F9
		private void OnStartLoot(InteractableLootbox lootbox)
		{
			HardwareSyncingManager.SetEvent("Interaction");
		}
	}
}
