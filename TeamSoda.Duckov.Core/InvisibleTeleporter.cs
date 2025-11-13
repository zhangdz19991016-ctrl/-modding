using System;
using Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020000A7 RID: 167
[Obsolete]
public class InvisibleTeleporter : MonoBehaviour, IDrawGizmos
{
	// Token: 0x1700011F RID: 287
	// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00019969 File Offset: 0x00017B69
	private bool UsePosition
	{
		get
		{
			return this.target == null;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00019978 File Offset: 0x00017B78
	private Vector3 TargetWorldPosition
	{
		get
		{
			if (this.target != null)
			{
				return this.target.transform.position;
			}
			Space space = this.space;
			if (space == Space.World)
			{
				return this.position;
			}
			if (space != Space.Self)
			{
				return default(Vector3);
			}
			return base.transform.TransformPoint(this.position);
		}
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x000199D8 File Offset: 0x00017BD8
	public void Teleport()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		GameCamera instance = GameCamera.Instance;
		Vector3 b = instance.transform.position - main.transform.position;
		main.SetPosition(this.TargetWorldPosition);
		Vector3 vector = main.transform.position + b;
		instance.transform.position = vector;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00019A3F File Offset: 0x00017C3F
	private void LateUpdate()
	{
		if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
		{
			this.Teleport();
		}
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00019A60 File Offset: 0x00017C60
	public void DrawGizmos()
	{
		if (!GizmoContext.InActiveSelection(this))
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			Draw.Arrow(base.transform.position, this.TargetWorldPosition);
			return;
		}
		Draw.Arrow(main.transform.position, this.TargetWorldPosition);
	}

	// Token: 0x04000530 RID: 1328
	[SerializeField]
	private Transform target;

	// Token: 0x04000531 RID: 1329
	[SerializeField]
	private Vector3 position;

	// Token: 0x04000532 RID: 1330
	[SerializeField]
	private Space space;
}
