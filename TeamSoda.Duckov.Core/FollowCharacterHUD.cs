using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class FollowCharacterHUD : MonoBehaviour
{
	// Token: 0x06000657 RID: 1623 RVA: 0x0001CA61 File Offset: 0x0001AC61
	private void Awake()
	{
		GameCamera.OnCameraPosUpdate = (Action<GameCamera, CharacterMainControl>)Delegate.Combine(GameCamera.OnCameraPosUpdate, new Action<GameCamera, CharacterMainControl>(this.UpdatePos));
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0001CA83 File Offset: 0x0001AC83
	private void OnDestroy()
	{
		GameCamera.OnCameraPosUpdate = (Action<GameCamera, CharacterMainControl>)Delegate.Remove(GameCamera.OnCameraPosUpdate, new Action<GameCamera, CharacterMainControl>(this.UpdatePos));
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0001CAA8 File Offset: 0x0001ACA8
	private void UpdatePos(GameCamera gameCamera, CharacterMainControl target)
	{
		Camera renderCamera = gameCamera.renderCamera;
		Vector3 vector = target.transform.position + this.offset;
		this.worldPos = Vector3.SmoothDamp(this.worldPos, vector, ref this.velocityTemp, this.smoothTime);
		if (Vector3.Distance(this.worldPos, vector) > this.maxDistance)
		{
			this.worldPos = (this.worldPos - vector).normalized * this.maxDistance + vector;
		}
		Vector3 position = renderCamera.WorldToScreenPoint(this.worldPos);
		base.transform.position = position;
		if (target.gameObject.activeInHierarchy != base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(target.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x04000611 RID: 1553
	public float maxDistance = 2f;

	// Token: 0x04000612 RID: 1554
	public float smoothTime;

	// Token: 0x04000613 RID: 1555
	private Vector3 worldPos;

	// Token: 0x04000614 RID: 1556
	private Vector3 velocityTemp;

	// Token: 0x04000615 RID: 1557
	public Vector3 offset;
}
