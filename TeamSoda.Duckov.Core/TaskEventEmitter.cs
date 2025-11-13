using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class TaskEventEmitter : MonoBehaviour
{
	// Token: 0x060009A3 RID: 2467 RVA: 0x0002A4ED File Offset: 0x000286ED
	public void SetKey(string key)
	{
		this.eventKey = key;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x0002A4F6 File Offset: 0x000286F6
	private void Awake()
	{
		if (this.emitOnAwake)
		{
			this.EmitEvent();
		}
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0002A506 File Offset: 0x00028706
	public void EmitEvent()
	{
		Debug.Log("TaskEvent:" + this.eventKey);
		TaskEvent.EmitTaskEvent(this.eventKey);
	}

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private string eventKey;

	// Token: 0x0400088E RID: 2190
	[SerializeField]
	private bool emitOnAwake;
}
