using System;
using Cysharp.Threading.Tasks;
using Duckov.UI.DialogueBubbles;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class DialogueBubbleProxy : MonoBehaviour
{
	// Token: 0x06000CE2 RID: 3298 RVA: 0x00036584 File Offset: 0x00034784
	public void Pop()
	{
		if (this.byMainCharacter && CharacterMainControl.Main)
		{
			CharacterMainControl.Main.PopText(this.textKey.ToPlainText(), -1f);
			return;
		}
		DialogueBubblesManager.Show(this.textKey.ToPlainText(), base.transform, this.yOffset, false, false, -1f, this.duration).Forget();
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x000365F0 File Offset: 0x000347F0
	public void Pop(string text, float speed = -1f)
	{
		if (this.byMainCharacter && CharacterMainControl.Main)
		{
			CharacterMainControl.Main.PopText(this.textKey.ToPlainText(), -1f);
			return;
		}
		DialogueBubblesManager.Show(text, base.transform, this.yOffset, false, false, speed, 2f).Forget();
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0003664B File Offset: 0x0003484B
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(base.transform.position + Vector3.up * this.yOffset, Vector3.one * 0.2f);
	}

	// Token: 0x04000B31 RID: 2865
	[LocalizationKey("Dialogues")]
	public string textKey;

	// Token: 0x04000B32 RID: 2866
	public float yOffset;

	// Token: 0x04000B33 RID: 2867
	public float duration = 2f;

	// Token: 0x04000B34 RID: 2868
	public bool byMainCharacter;
}
