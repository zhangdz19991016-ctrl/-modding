using System;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x0200026E RID: 622
	public abstract class ModBehaviour : MonoBehaviour
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00048CD6 File Offset: 0x00046ED6
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x00048CDE File Offset: 0x00046EDE
		public ModManager master { get; private set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x00048CE7 File Offset: 0x00046EE7
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x00048CEF File Offset: 0x00046EEF
		public ModInfo info { get; private set; }

		// Token: 0x06001375 RID: 4981 RVA: 0x00048CF8 File Offset: 0x00046EF8
		public void Setup(ModManager master, ModInfo info)
		{
			this.master = master;
			this.info = info;
			this.OnAfterSetup();
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00048D0E File Offset: 0x00046F0E
		public void NotifyBeforeDeactivate()
		{
			this.OnBeforeDeactivate();
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00048D16 File Offset: 0x00046F16
		protected virtual void OnAfterSetup()
		{
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00048D18 File Offset: 0x00046F18
		protected virtual void OnBeforeDeactivate()
		{
		}
	}
}
