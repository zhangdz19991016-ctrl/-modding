using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class LittleMoveHUD : MonoBehaviour
{
	// Token: 0x0600065B RID: 1627 RVA: 0x0001CB88 File Offset: 0x0001AD88
	private void LateUpdate()
	{
		if (!this.character)
		{
			if (LevelManager.Instance)
			{
				this.character = LevelManager.Instance.MainCharacter;
			}
			if (!this.character)
			{
				return;
			}
		}
		if (!this.camera)
		{
			this.camera = Camera.main;
			if (!this.camera)
			{
				return;
			}
		}
		Vector3 vector = this.character.transform.position + this.offset;
		this.worldPos = Vector3.SmoothDamp(this.worldPos, vector, ref this.velocityTemp, this.smoothTime);
		if (Vector3.Distance(this.worldPos, vector) > this.maxDistance)
		{
			this.worldPos = (this.worldPos - vector).normalized * this.maxDistance + vector;
		}
		Vector3 position = this.camera.WorldToScreenPoint(this.worldPos);
		base.transform.position = position;
	}

	// Token: 0x04000616 RID: 1558
	private Camera camera;

	// Token: 0x04000617 RID: 1559
	private CharacterMainControl character;

	// Token: 0x04000618 RID: 1560
	public float maxDistance = 2f;

	// Token: 0x04000619 RID: 1561
	public float smoothTime;

	// Token: 0x0400061A RID: 1562
	private Vector3 worldPos;

	// Token: 0x0400061B RID: 1563
	private Vector3 velocityTemp;

	// Token: 0x0400061C RID: 1564
	public Vector3 offset;
}
