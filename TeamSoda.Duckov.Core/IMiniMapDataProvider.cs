using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public interface IMiniMapDataProvider
{
	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000D7E RID: 3454
	Sprite CombinedSprite { get; }

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000D7F RID: 3455
	List<IMiniMapEntry> Maps { get; }

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000D80 RID: 3456
	float PixelSize { get; }

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000D81 RID: 3457
	Vector3 CombinedCenter { get; }
}
