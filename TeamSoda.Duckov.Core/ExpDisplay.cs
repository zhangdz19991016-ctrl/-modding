using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FA RID: 506
public class ExpDisplay : MonoBehaviour
{
	// Token: 0x06000EE6 RID: 3814 RVA: 0x0003BDA0 File Offset: 0x00039FA0
	private void Refresh()
	{
		EXPManager instance = EXPManager.Instance;
		if (instance == null)
		{
			return;
		}
		int num = instance.LevelFromExp(this.displayExp);
		if (this.displayingLevel != num)
		{
			this.displayingLevel = num;
			this.OnDisplayingLevelChanged();
		}
		ValueTuple<long, long> levelExpRange = this.GetLevelExpRange(num);
		long num2 = levelExpRange.Item2 - levelExpRange.Item1;
		this.txtLevel.text = num.ToString();
		this.txtCurrentExp.text = this.displayExp.ToString();
		string text;
		if (levelExpRange.Item2 == 9223372036854775807L)
		{
			text = "∞";
		}
		else
		{
			text = levelExpRange.Item2.ToString();
		}
		this.txtMaxExp.text = text;
		float fillAmount = (float)((double)(this.displayExp - levelExpRange.Item1) / (double)num2);
		this.expBarFill.fillAmount = fillAmount;
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0003BE74 File Offset: 0x0003A074
	private void OnDisplayingLevelChanged()
	{
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0003BE78 File Offset: 0x0003A078
	[return: TupleElementNames(new string[]
	{
		"from",
		"to"
	})]
	private ValueTuple<long, long> GetLevelExpRange(int level)
	{
		ValueTuple<long, long> result;
		if (this.cachedLevelExpRange.TryGetValue(level, out result))
		{
			return result;
		}
		EXPManager instance = EXPManager.Instance;
		if (instance == null)
		{
			return new ValueTuple<long, long>(0L, 0L);
		}
		ValueTuple<long, long> levelExpRange = instance.GetLevelExpRange(level);
		this.cachedLevelExpRange[level] = levelExpRange;
		return levelExpRange;
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0003BEC6 File Offset: 0x0003A0C6
	private void SnapToCurrent()
	{
		this.displayExp = EXPManager.EXP;
		this.Refresh();
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0003BEDC File Offset: 0x0003A0DC
	private UniTask Animate(long targetExp, float duration, AnimationCurve curve)
	{
		ExpDisplay.<Animate>d__15 <Animate>d__;
		<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Animate>d__.<>4__this = this;
		<Animate>d__.targetExp = targetExp;
		<Animate>d__.duration = duration;
		<Animate>d__.curve = curve;
		<Animate>d__.<>1__state = -1;
		<Animate>d__.<>t__builder.Start<ExpDisplay.<Animate>d__15>(ref <Animate>d__);
		return <Animate>d__.<>t__builder.Task;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0003BF38 File Offset: 0x0003A138
	private long LongLerp(long a, long b, float t)
	{
		long num = b - a;
		return a + (long)(t * (float)num);
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0003BF50 File Offset: 0x0003A150
	private void OnEnable()
	{
		if (this.snapToCurrentOnEnable)
		{
			this.SnapToCurrent();
		}
		this.RegisterEvents();
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0003BF66 File Offset: 0x0003A166
	private void OnDisable()
	{
		this.UnregisterEvents();
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0003BF6E File Offset: 0x0003A16E
	private void RegisterEvents()
	{
		EXPManager.onExpChanged = (Action<long>)Delegate.Combine(EXPManager.onExpChanged, new Action<long>(this.OnExpChanged));
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0003BF90 File Offset: 0x0003A190
	private void UnregisterEvents()
	{
		EXPManager.onExpChanged = (Action<long>)Delegate.Remove(EXPManager.onExpChanged, new Action<long>(this.OnExpChanged));
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x0003BFB2 File Offset: 0x0003A1B2
	private void OnExpChanged(long exp)
	{
		this.Animate(exp, this.animationDuration, this.animationCurve).Forget();
	}

	// Token: 0x04000C55 RID: 3157
	[SerializeField]
	private TextMeshProUGUI txtLevel;

	// Token: 0x04000C56 RID: 3158
	[SerializeField]
	private TextMeshProUGUI txtCurrentExp;

	// Token: 0x04000C57 RID: 3159
	[SerializeField]
	private TextMeshProUGUI txtMaxExp;

	// Token: 0x04000C58 RID: 3160
	[SerializeField]
	private Image expBarFill;

	// Token: 0x04000C59 RID: 3161
	[SerializeField]
	private bool snapToCurrentOnEnable;

	// Token: 0x04000C5A RID: 3162
	[SerializeField]
	private float animationDuration = 0.1f;

	// Token: 0x04000C5B RID: 3163
	[SerializeField]
	private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000C5C RID: 3164
	[SerializeField]
	private long displayExp;

	// Token: 0x04000C5D RID: 3165
	private int displayingLevel = -1;

	// Token: 0x04000C5E RID: 3166
	[TupleElementNames(new string[]
	{
		"from",
		"to"
	})]
	private Dictionary<int, ValueTuple<long, long>> cachedLevelExpRange = new Dictionary<int, ValueTuple<long, long>>();

	// Token: 0x04000C5F RID: 3167
	private int currentToken;
}
