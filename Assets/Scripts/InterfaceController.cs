using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InterfaceController : MonoBehaviour {

	public static InterfaceController instance;

	[SerializeField] Animator buttonAnimator;
	[SerializeField] Animator clockLayoutAnimator;
	[SerializeField] Text clockText;

	bool appStarted;
	DateTime lastShownTime;
	public event Action eEvenSecond;

	void Awake()
	{
		instance = this;
		lastShownTime = DateTime.UtcNow;
		if (!buttonAnimator.gameObject.activeSelf) buttonAnimator.gameObject.SetActive(true);
		if (clockLayoutAnimator.gameObject.activeSelf) clockLayoutAnimator.gameObject.SetActive(false);
	}

	//Button methods
	public void Bn_StartApp()
	{
		appStarted = true;
		buttonAnimator.Play("ButtonFade");
		clockLayoutAnimator.gameObject.SetActive(true);
		clockLayoutAnimator.Play("LayoutFadeIn");
	}

	void Update()
	{
		if (appStarted)
		{
			DateTime timeCheck = DateTime.UtcNow;
			if (HasTimeChanged(timeCheck))
			{
				//clockText.text = string.Format("{0}:{1}:{2}", timeCheck.Hour, timeCheck.Minute, timeCheck.Second);
				clockText.text = timeCheck.ToString("hh:mm:ss");
				lastShownTime = timeCheck;

				//Check is seconds are even
				if(timeCheck.Second % 2 == 0)
				{
					//dispatch an event to any class that needs it
					if (eEvenSecond != null) eEvenSecond();
				}
			}
		}
	}

	/// <summary>
	/// Calculates if time difference is big enough to render shown time again
	/// </summary>
	bool HasTimeChanged(DateTime time)
	{
		return (time - lastShownTime).TotalSeconds >= 1;
	}
}
