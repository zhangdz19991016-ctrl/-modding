using System;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class HandheldSprite : MonoBehaviour
{
	// Token: 0x06000BA5 RID: 2981 RVA: 0x00031B1B File Offset: 0x0002FD1B
	private void Start()
	{
		if (this.agent.Item)
		{
			this.spriteRenderer.sprite = this.agent.Item.Icon;
		}
	}

	// Token: 0x040009F7 RID: 2551
	public DuckovItemAgent agent;

	// Token: 0x040009F8 RID: 2552
	public SpriteRenderer spriteRenderer;
}
