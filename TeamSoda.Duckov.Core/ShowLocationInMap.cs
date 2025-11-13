using System;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class ShowLocationInMap : MonoBehaviour
{
	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0002B056 File Offset: 0x00029256
	public string DisplayName
	{
		get
		{
			return this.displayName;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0002B05E File Offset: 0x0002925E
	public string DisplayNameRaw
	{
		get
		{
			return this.displayName;
		}
	}

	// Token: 0x040008B6 RID: 2230
	[SerializeField]
	private string displayName;
}
