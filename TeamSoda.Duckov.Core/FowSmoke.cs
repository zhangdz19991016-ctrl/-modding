using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A5 RID: 165
public class FowSmoke : MonoBehaviour
{
	// Token: 0x060005AC RID: 1452 RVA: 0x00019838 File Offset: 0x00017A38
	private void Start()
	{
		this.UpdateSmoke().Forget();
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00019854 File Offset: 0x00017A54
	private UniTaskVoid UpdateSmoke()
	{
		FowSmoke.<UpdateSmoke>d__11 <UpdateSmoke>d__;
		<UpdateSmoke>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<UpdateSmoke>d__.<>4__this = this;
		<UpdateSmoke>d__.<>1__state = -1;
		<UpdateSmoke>d__.<>t__builder.Start<FowSmoke.<UpdateSmoke>d__11>(ref <UpdateSmoke>d__);
		return <UpdateSmoke>d__.<>t__builder.Task;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00019897 File Offset: 0x00017A97
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}

	// Token: 0x04000522 RID: 1314
	[SerializeField]
	private int res = 8;

	// Token: 0x04000523 RID: 1315
	[SerializeField]
	private float radius;

	// Token: 0x04000524 RID: 1316
	[SerializeField]
	private float height;

	// Token: 0x04000525 RID: 1317
	[SerializeField]
	private float thickness;

	// Token: 0x04000526 RID: 1318
	public Transform colParent;

	// Token: 0x04000527 RID: 1319
	public ParticleSystem[] particles;

	// Token: 0x04000528 RID: 1320
	public float startTime;

	// Token: 0x04000529 RID: 1321
	public float lifeTime;

	// Token: 0x0400052A RID: 1322
	public float particleFadeTime = 3f;

	// Token: 0x0400052B RID: 1323
	public UnityEvent beforeFadeOutEvent;
}
