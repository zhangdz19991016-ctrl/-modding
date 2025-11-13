using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class UIInputEventData
{
	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00030CAF File Offset: 0x0002EEAF
	public bool Used
	{
		get
		{
			return this.used;
		}
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00030CB7 File Offset: 0x0002EEB7
	public void Use()
	{
		this.used = true;
	}

	// Token: 0x040009C4 RID: 2500
	private bool used;

	// Token: 0x040009C5 RID: 2501
	public Vector2 vector;

	// Token: 0x040009C6 RID: 2502
	public bool confirm;

	// Token: 0x040009C7 RID: 2503
	public bool cancel;
}
