using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AE RID: 942
	public abstract class ManagedUIElement : MonoBehaviour
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060021D4 RID: 8660 RVA: 0x000769DC File Offset: 0x00074BDC
		// (set) Token: 0x060021D5 RID: 8661 RVA: 0x000769E4 File Offset: 0x00074BE4
		public bool open { get; private set; }

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x060021D6 RID: 8662 RVA: 0x000769F0 File Offset: 0x00074BF0
		// (remove) Token: 0x060021D7 RID: 8663 RVA: 0x00076A24 File Offset: 0x00074C24
		public static event Action<ManagedUIElement> onOpen;

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x060021D8 RID: 8664 RVA: 0x00076A58 File Offset: 0x00074C58
		// (remove) Token: 0x060021D9 RID: 8665 RVA: 0x00076A8C File Offset: 0x00074C8C
		public static event Action<ManagedUIElement> onClose;

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x00076ABF File Offset: 0x00074CBF
		protected virtual bool ShowOpenCloseButtons
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00076AC2 File Offset: 0x00074CC2
		protected virtual void Awake()
		{
			this.RegisterEvents();
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x00076ACA File Offset: 0x00074CCA
		protected virtual void OnDestroy()
		{
			this.UnregisterEvents();
			if (this.open)
			{
				this.Close();
			}
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00076AE0 File Offset: 0x00074CE0
		public void Open(ManagedUIElement parent = null)
		{
			this.open = true;
			this.parent = parent;
			Action<ManagedUIElement> action = ManagedUIElement.onOpen;
			if (action != null)
			{
				action(this);
			}
			this.OnOpen();
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00076B07 File Offset: 0x00074D07
		public void Close()
		{
			this.open = false;
			this.parent = null;
			Action<ManagedUIElement> action = ManagedUIElement.onClose;
			if (action != null)
			{
				action(this);
			}
			this.OnClose();
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x00076B2E File Offset: 0x00074D2E
		private void RegisterEvents()
		{
			ManagedUIElement.onOpen += this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose += this.OnManagedUIBehaviorClose;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x00076B52 File Offset: 0x00074D52
		private void UnregisterEvents()
		{
			ManagedUIElement.onOpen -= this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose -= this.OnManagedUIBehaviorClose;
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x00076B76 File Offset: 0x00074D76
		private void OnManagedUIBehaviorClose(ManagedUIElement obj)
		{
			if (obj != null && obj == this.parent)
			{
				this.Close();
			}
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x00076B95 File Offset: 0x00074D95
		private void OnManagedUIBehaviorOpen(ManagedUIElement obj)
		{
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00076B97 File Offset: 0x00074D97
		protected virtual void OnOpen()
		{
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x00076B99 File Offset: 0x00074D99
		protected virtual void OnClose()
		{
		}

		// Token: 0x040016E8 RID: 5864
		[SerializeField]
		private ManagedUIElement parent;
	}
}
