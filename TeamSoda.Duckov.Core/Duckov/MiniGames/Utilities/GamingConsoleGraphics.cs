using System;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x0200028C RID: 652
	public class GamingConsoleGraphics : MonoBehaviour
	{
		// Token: 0x06001507 RID: 5383 RVA: 0x0004E49C File Offset: 0x0004C69C
		private void Awake()
		{
			this.master.onContentChanged += this.OnContentChanged;
			this.master.OnAfterAnimateIn += this.OnAfterAnimateIn;
			this.master.OnBeforeAnimateOut += this.OnBeforeAnimateOut;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0004E4EE File Offset: 0x0004C6EE
		private void Start()
		{
			this.dirty = true;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0004E4F8 File Offset: 0x0004C6F8
		private void OnContentChanged(GamingConsole console)
		{
			if (console.Monitor != this._cachedMonitor)
			{
				this.OnMonitorChanged();
			}
			if (console.Console != this._cachedConsole)
			{
				this.OnConsoleChanged();
			}
			if (console.Cartridge != this._cachedCartridge)
			{
				this.OnCatridgeChanged();
			}
			this.dirty = true;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0004E557 File Offset: 0x0004C757
		private void Update()
		{
			if (this.dirty)
			{
				this.RefreshDisplays();
				this.dirty = false;
			}
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0004E570 File Offset: 0x0004C770
		private void RefreshDisplays()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this._cachedMonitor = this.master.Monitor;
			this._cachedConsole = this.master.Console;
			this._cachedCartridge = this.master.Cartridge;
			if (this.monitorGraphic)
			{
				UnityEngine.Object.Destroy(this.monitorGraphic.gameObject);
			}
			if (this.consoleGraphic)
			{
				UnityEngine.Object.Destroy(this.consoleGraphic.gameObject);
			}
			if (this._cachedMonitor && !this._cachedMonitor.IsBeingDestroyed)
			{
				this.monitorGraphic = ItemGraphicInfo.CreateAGraphic(this._cachedMonitor, this.monitorRoot);
			}
			if (this._cachedConsole && !this._cachedConsole.IsBeingDestroyed)
			{
				this.consoleGraphic = ItemGraphicInfo.CreateAGraphic(this._cachedConsole, this.consoleRoot);
				if (this.consoleGraphic != null)
				{
					this.pickupAnimation = this.consoleGraphic.GetComponent<ControllerPickupAnimation>();
					this.controllerAnimator = this.consoleGraphic.GetComponentInChildren<ControllerAnimator>();
				}
				else
				{
					this.pickupAnimation = null;
					this.controllerAnimator = null;
				}
				if (this.controllerAnimator != null)
				{
					this.controllerAnimator.SetConsole(this.master);
				}
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0004E6B7 File Offset: 0x0004C8B7
		private void OnCatridgeChanged()
		{
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0004E6B9 File Offset: 0x0004C8B9
		private void OnConsoleChanged()
		{
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0004E6BB File Offset: 0x0004C8BB
		private void OnMonitorChanged()
		{
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0004E6BD File Offset: 0x0004C8BD
		private void OnDestroy()
		{
			this.isBeingDestroyed = true;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0004E6C6 File Offset: 0x0004C8C6
		private void OnBeforeAnimateOut(GamingConsole console)
		{
			if (this.pickupAnimation == null)
			{
				return;
			}
			this.pickupAnimation.PutDown().Forget();
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0004E6E7 File Offset: 0x0004C8E7
		private void OnAfterAnimateIn(GamingConsole console)
		{
			if (this.pickupAnimation == null)
			{
				return;
			}
			this.pickupAnimation.PickUp(this.playingControllerPosition).Forget();
		}

		// Token: 0x04000F60 RID: 3936
		[SerializeField]
		private GamingConsole master;

		// Token: 0x04000F61 RID: 3937
		[SerializeField]
		private Transform monitorRoot;

		// Token: 0x04000F62 RID: 3938
		[SerializeField]
		private Transform consoleRoot;

		// Token: 0x04000F63 RID: 3939
		[SerializeField]
		private Transform playingControllerPosition;

		// Token: 0x04000F64 RID: 3940
		private Transform cartridgeRoot;

		// Token: 0x04000F65 RID: 3941
		private Item _cachedMonitor;

		// Token: 0x04000F66 RID: 3942
		private Item _cachedConsole;

		// Token: 0x04000F67 RID: 3943
		private Item _cachedCartridge;

		// Token: 0x04000F68 RID: 3944
		private ItemGraphicInfo monitorGraphic;

		// Token: 0x04000F69 RID: 3945
		private ItemGraphicInfo consoleGraphic;

		// Token: 0x04000F6A RID: 3946
		private ControllerPickupAnimation pickupAnimation;

		// Token: 0x04000F6B RID: 3947
		private ControllerAnimator controllerAnimator;

		// Token: 0x04000F6C RID: 3948
		private bool dirty;

		// Token: 0x04000F6D RID: 3949
		private bool isBeingDestroyed;
	}
}
