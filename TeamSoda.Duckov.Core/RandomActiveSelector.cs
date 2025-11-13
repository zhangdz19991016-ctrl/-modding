using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class RandomActiveSelector : MonoBehaviour
{
	// Token: 0x060005C8 RID: 1480 RVA: 0x00019FB8 File Offset: 0x000181B8
	private void Awake()
	{
		foreach (GameObject gameObject in this.selections)
		{
			if (!(gameObject == null))
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0001A014 File Offset: 0x00018214
	private void Update()
	{
		if (!this.setted && LevelManager.LevelInited)
		{
			this.Set();
		}
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0001A02C File Offset: 0x0001822C
	private void Set()
	{
		if (MultiSceneCore.Instance == null)
		{
			return;
		}
		object obj;
		if (MultiSceneCore.Instance.inLevelData.TryGetValue(this.guid, out obj))
		{
			this.activeIndex = (int)obj;
		}
		else
		{
			if (UnityEngine.Random.Range(0f, 1f) > this.activeChance)
			{
				this.activeIndex = -1;
			}
			else
			{
				this.activeIndex = UnityEngine.Random.Range(0, this.selections.Count);
			}
			MultiSceneCore.Instance.inLevelData.Add(this.guid, this.activeIndex);
		}
		if (this.activeIndex >= 0)
		{
			GameObject gameObject = this.selections[this.activeIndex];
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
		this.setted = true;
		base.enabled = false;
	}

	// Token: 0x04000547 RID: 1351
	[Range(0f, 1f)]
	public float activeChance = 1f;

	// Token: 0x04000548 RID: 1352
	private int activeIndex;

	// Token: 0x04000549 RID: 1353
	private int guid;

	// Token: 0x0400054A RID: 1354
	private bool setted;

	// Token: 0x0400054B RID: 1355
	public List<GameObject> selections;
}
