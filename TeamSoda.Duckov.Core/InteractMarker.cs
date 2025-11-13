using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class InteractMarker : MonoBehaviour
{
	// Token: 0x06000720 RID: 1824 RVA: 0x00020448 File Offset: 0x0001E648
	public void MarkAsUsed()
	{
		if (this.markedAsUsed)
		{
			return;
		}
		this.markedAsUsed = true;
		if (this.hideIfUsedObject)
		{
			this.hideIfUsedObject.SetActive(false);
		}
		if (this.showIfUsedObject)
		{
			this.showIfUsedObject.SetActive(true);
		}
	}

	// Token: 0x040006C5 RID: 1733
	private bool markedAsUsed;

	// Token: 0x040006C6 RID: 1734
	public GameObject showIfUsedObject;

	// Token: 0x040006C7 RID: 1735
	public GameObject hideIfUsedObject;
}
