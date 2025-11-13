using System;
using ItemStatsSystem;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class FxAction : EffectAction
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060004CC RID: 1228 RVA: 0x00015E37 File Offset: 0x00014037
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00015E74 File Offset: 0x00014074
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl || !this.MainControl.characterModel)
		{
			return;
		}
		Transform transform = this.MainControl.transform;
		switch (this.socket)
		{
		case FxAction.Sockets.root:
			break;
		case FxAction.Sockets.helmat:
			transform = this.MainControl.characterModel.HelmatSocket;
			break;
		case FxAction.Sockets.armor:
			transform = this.MainControl.characterModel.ArmorSocket;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!transform)
		{
			return;
		}
		if (!this.fxPfb)
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(this.fxPfb, transform.position, quaternion.identity);
	}

	// Token: 0x04000406 RID: 1030
	public FxAction.Sockets socket = FxAction.Sockets.helmat;

	// Token: 0x04000407 RID: 1031
	public GameObject fxPfb;

	// Token: 0x04000408 RID: 1032
	private CharacterMainControl _mainControl;

	// Token: 0x02000445 RID: 1093
	public enum Sockets
	{
		// Token: 0x04001AAE RID: 6830
		root,
		// Token: 0x04001AAF RID: 6831
		helmat,
		// Token: 0x04001AB0 RID: 6832
		armor
	}
}
