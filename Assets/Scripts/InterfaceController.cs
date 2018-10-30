using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InterfaceController : MonoBehaviour {

	/* Class controls the app UI. Main features:
	 * - display current UTC time
	 * - dispatch one event every even second
	 */

	public static InterfaceController instance;

	[Header("Variables for Inspector")]
	[SerializeField] Animator buttonAnimator;
	[SerializeField] Animator clockLayoutAnimator;
	[SerializeField] Text clockText;

	bool appStarted;
	DateTime lastShownTime;
	public event Action eEvenSecond;

	void Awake()
	{
		//Prepare class and canvases
		instance = this;
		lastShownTime = DateTime.UtcNow;
		clockText.text = lastShownTime.ToString("hh:mm:ss");
		if (!buttonAnimator.gameObject.activeSelf) buttonAnimator.gameObject.SetActive(true);
		if (clockLayoutAnimator.gameObject.activeSelf) clockLayoutAnimator.gameObject.SetActive(false);
	}

	/// <summary>
	/// Button method for UI
	/// </summary>
	public void Bn_StartApp()
	{
		appStarted = true;
		buttonAnimator.Play("ButtonFade");
		clockLayoutAnimator.gameObject.SetActive(true);
		clockLayoutAnimator.Play("LayoutFadeIn");
	}

	void Update()
	{
	    if (!appStarted) return;
	    //Save time ignoring miliseconds for time difference calculation purposes
	    var ut = DateTime.UtcNow;
	    var timeCheck = new DateTime(ut.Year, ut.Month, ut.Day, ut.Hour, ut.Minute, ut.Second);
	    if (!HasTimeChanged(timeCheck)) return;
	    clockText.text = timeCheck.ToString("hh:mm:ss");
	    lastShownTime = timeCheck;

	    //Check if seconds are even
	    if (timeCheck.Second % 2 != 0) return;
	    //dispatch an event to any class that needs it
	    if (eEvenSecond != null) eEvenSecond();
	}

	/// <summary>
	/// Calculates if time difference is big enough to render shown time again
	/// </summary>
	bool HasTimeChanged(DateTime time)
	{
		return (time - lastShownTime).TotalSeconds >= 1;
	}
}
