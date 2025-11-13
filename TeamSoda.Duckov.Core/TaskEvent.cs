using System;

// Token: 0x02000122 RID: 290
public class TaskEvent
{
	// Token: 0x14000048 RID: 72
	// (add) Token: 0x0600099F RID: 2463 RVA: 0x0002A46C File Offset: 0x0002866C
	// (remove) Token: 0x060009A0 RID: 2464 RVA: 0x0002A4A0 File Offset: 0x000286A0
	public static event Action<string> OnTaskEvent;

	// Token: 0x060009A1 RID: 2465 RVA: 0x0002A4D3 File Offset: 0x000286D3
	public static void EmitTaskEvent(string taskEventKey)
	{
		Action<string> onTaskEvent = TaskEvent.OnTaskEvent;
		if (onTaskEvent == null)
		{
			return;
		}
		onTaskEvent(taskEventKey);
	}
}
