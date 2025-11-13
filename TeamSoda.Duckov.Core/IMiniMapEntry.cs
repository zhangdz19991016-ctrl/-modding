using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public interface IMiniMapEntry
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000D82 RID: 3458
	Sprite Sprite { get; }

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000D83 RID: 3459
	float PixelSize { get; }

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000D84 RID: 3460
	Vector2 Offset { get; }

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000D85 RID: 3461
	string SceneID { get; }

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000D86 RID: 3462
	bool Hide { get; }

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000D87 RID: 3463
	bool NoSignal { get; }
}
