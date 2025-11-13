using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class SpawnPaperBoxAction : EffectAction
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060004DA RID: 1242 RVA: 0x000161A7 File Offset: 0x000143A7
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

	// Token: 0x060004DB RID: 1243 RVA: 0x000161E4 File Offset: 0x000143E4
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl || !this.MainControl.characterModel)
		{
			return;
		}
		Transform transform = this.MainControl.transform;
		switch (this.socket)
		{
		case SpawnPaperBoxAction.Sockets.root:
			break;
		case SpawnPaperBoxAction.Sockets.helmat:
			transform = this.MainControl.characterModel.HelmatSocket;
			break;
		case SpawnPaperBoxAction.Sockets.armor:
			transform = this.MainControl.characterModel.ArmorSocket;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!transform)
		{
			return;
		}
		if (!this.paperBoxPrefab)
		{
			return;
		}
		this.instance = UnityEngine.Object.Instantiate<PaperBox>(this.paperBoxPrefab, transform);
		this.instance.character = this.MainControl;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0001629E File Offset: 0x0001449E
	private void OnDestroy()
	{
		if (this.instance)
		{
			UnityEngine.Object.Destroy(this.instance.gameObject);
		}
	}

	// Token: 0x04000416 RID: 1046
	public SpawnPaperBoxAction.Sockets socket = SpawnPaperBoxAction.Sockets.helmat;

	// Token: 0x04000417 RID: 1047
	public PaperBox paperBoxPrefab;

	// Token: 0x04000418 RID: 1048
	private PaperBox instance;

	// Token: 0x04000419 RID: 1049
	private CharacterMainControl _mainControl;

	// Token: 0x02000446 RID: 1094
	public enum Sockets
	{
		// Token: 0x04001AB2 RID: 6834
		root,
		// Token: 0x04001AB3 RID: 6835
		helmat,
		// Token: 0x04001AB4 RID: 6836
		armor
	}
}
