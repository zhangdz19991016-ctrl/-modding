using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class PackedMapData : ScriptableObject, IMiniMapDataProvider
{
	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0003878C File Offset: 0x0003698C
	public Sprite CombinedSprite
	{
		get
		{
			return this.combinedSprite;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00038794 File Offset: 0x00036994
	public float PixelSize
	{
		get
		{
			return this.pixelSize;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0003879C File Offset: 0x0003699C
	public Vector3 CombinedCenter
	{
		get
		{
			return this.combinedCenter;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000D97 RID: 3479 RVA: 0x000387A4 File Offset: 0x000369A4
	public List<IMiniMapEntry> Maps
	{
		get
		{
			return this.maps.ToList<IMiniMapEntry>();
		}
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x000387B4 File Offset: 0x000369B4
	internal void Setup(IMiniMapDataProvider origin)
	{
		this.combinedSprite = origin.CombinedSprite;
		this.pixelSize = origin.PixelSize;
		this.combinedCenter = origin.CombinedCenter;
		this.maps.Clear();
		foreach (IMiniMapEntry miniMapEntry in origin.Maps)
		{
			PackedMapData.Entry item = new PackedMapData.Entry(miniMapEntry.Sprite, miniMapEntry.PixelSize, miniMapEntry.Offset, miniMapEntry.SceneID, miniMapEntry.Hide, miniMapEntry.NoSignal);
			this.maps.Add(item);
		}
	}

	// Token: 0x04000B94 RID: 2964
	[SerializeField]
	private Sprite combinedSprite;

	// Token: 0x04000B95 RID: 2965
	[SerializeField]
	private float pixelSize;

	// Token: 0x04000B96 RID: 2966
	[SerializeField]
	private Vector3 combinedCenter;

	// Token: 0x04000B97 RID: 2967
	[SerializeField]
	private List<PackedMapData.Entry> maps = new List<PackedMapData.Entry>();

	// Token: 0x020004DD RID: 1245
	[Serializable]
	public class Entry : IMiniMapEntry
	{
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002789 RID: 10121 RVA: 0x0008FA39 File Offset: 0x0008DC39
		public Sprite Sprite
		{
			get
			{
				return this.sprite;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x0008FA41 File Offset: 0x0008DC41
		public float PixelSize
		{
			get
			{
				return this.pixelSize;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x0008FA49 File Offset: 0x0008DC49
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x0008FA51 File Offset: 0x0008DC51
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x0008FA59 File Offset: 0x0008DC59
		public bool Hide
		{
			get
			{
				return this.hide;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x0008FA61 File Offset: 0x0008DC61
		public bool NoSignal
		{
			get
			{
				return this.noSignal;
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x0008FA69 File Offset: 0x0008DC69
		public Entry()
		{
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x0008FA71 File Offset: 0x0008DC71
		public Entry(Sprite sprite, float pixelSize, Vector2 offset, string sceneID, bool hide, bool noSignal)
		{
			this.sprite = sprite;
			this.pixelSize = pixelSize;
			this.offset = offset;
			this.sceneID = sceneID;
			this.hide = hide;
			this.noSignal = noSignal;
		}

		// Token: 0x04001D31 RID: 7473
		[SerializeField]
		private Sprite sprite;

		// Token: 0x04001D32 RID: 7474
		[SerializeField]
		private float pixelSize;

		// Token: 0x04001D33 RID: 7475
		[SerializeField]
		private Vector2 offset;

		// Token: 0x04001D34 RID: 7476
		[SerializeField]
		private string sceneID;

		// Token: 0x04001D35 RID: 7477
		[SerializeField]
		private bool hide;

		// Token: 0x04001D36 RID: 7478
		[SerializeField]
		private bool noSignal;
	}
}
