using System;
using System.Collections.Generic;
using Duckov.MiniMaps;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C4 RID: 452
public class MapMarkerSettingsPanel : MonoBehaviour
{
	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00038537 File Offset: 0x00036737
	private List<Sprite> Icons
	{
		get
		{
			return MapMarkerManager.Icons;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00038540 File Offset: 0x00036740
	private PrefabPool<MapMarkerPanelButton> IconBtnPool
	{
		get
		{
			if (this._iconBtnPool == null)
			{
				this._iconBtnPool = new PrefabPool<MapMarkerPanelButton>(this.iconBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._iconBtnPool;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000D8D RID: 3469 RVA: 0x0003857C File Offset: 0x0003677C
	private PrefabPool<MapMarkerPanelButton> ColorBtnPool
	{
		get
		{
			if (this._colorBtnPool == null)
			{
				this._colorBtnPool = new PrefabPool<MapMarkerPanelButton>(this.colorBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._colorBtnPool;
		}
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x000385B8 File Offset: 0x000367B8
	private void OnEnable()
	{
		this.Setup();
		MapMarkerManager.OnColorChanged = (Action<Color>)Delegate.Combine(MapMarkerManager.OnColorChanged, new Action<Color>(this.OnColorChanged));
		MapMarkerManager.OnIconChanged = (Action<int>)Delegate.Combine(MapMarkerManager.OnIconChanged, new Action<int>(this.OnIconChanged));
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0003860C File Offset: 0x0003680C
	private void OnDisable()
	{
		MapMarkerManager.OnColorChanged = (Action<Color>)Delegate.Remove(MapMarkerManager.OnColorChanged, new Action<Color>(this.OnColorChanged));
		MapMarkerManager.OnIconChanged = (Action<int>)Delegate.Remove(MapMarkerManager.OnIconChanged, new Action<int>(this.OnIconChanged));
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00038659 File Offset: 0x00036859
	private void OnIconChanged(int obj)
	{
		this.Setup();
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00038661 File Offset: 0x00036861
	private void OnColorChanged(Color color)
	{
		this.Setup();
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x0003866C File Offset: 0x0003686C
	private void Setup()
	{
		if (MapMarkerManager.Instance == null)
		{
			return;
		}
		this.IconBtnPool.ReleaseAll();
		this.ColorBtnPool.ReleaseAll();
		Color[] array = this.colors;
		for (int i = 0; i < array.Length; i++)
		{
			Color cur = array[i];
			MapMarkerPanelButton mapMarkerPanelButton = this.ColorBtnPool.Get(null);
			mapMarkerPanelButton.Image.color = cur;
			mapMarkerPanelButton.Setup(delegate
			{
				MapMarkerManager.SelectColor(cur);
			}, cur == MapMarkerManager.SelectedColor);
		}
		for (int j = 0; j < this.Icons.Count; j++)
		{
			Sprite sprite = this.Icons[j];
			if (!(sprite == null))
			{
				MapMarkerPanelButton mapMarkerPanelButton2 = this.IconBtnPool.Get(null);
				Image image = mapMarkerPanelButton2.Image;
				image.sprite = sprite;
				image.color = MapMarkerManager.SelectedColor;
				int index = j;
				mapMarkerPanelButton2.Setup(delegate
				{
					MapMarkerManager.SelectIcon(index);
				}, index == MapMarkerManager.SelectedIconIndex);
			}
		}
	}

	// Token: 0x04000B8F RID: 2959
	[SerializeField]
	private Color[] colors;

	// Token: 0x04000B90 RID: 2960
	[SerializeField]
	private MapMarkerPanelButton iconBtnTemplate;

	// Token: 0x04000B91 RID: 2961
	[SerializeField]
	private MapMarkerPanelButton colorBtnTemplate;

	// Token: 0x04000B92 RID: 2962
	private PrefabPool<MapMarkerPanelButton> _iconBtnPool;

	// Token: 0x04000B93 RID: 2963
	private PrefabPool<MapMarkerPanelButton> _colorBtnPool;
}
