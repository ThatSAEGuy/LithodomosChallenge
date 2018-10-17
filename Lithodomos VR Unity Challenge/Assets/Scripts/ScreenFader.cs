using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour 
{
	/// <summary>
	/// Rather than use two separate bools and risk having them both set to true at the same time (and cause a fade to get stuck),
	/// we use an enum to determine whether the screen is currently fading to black, fading out of black, or not fading at all.
	/// </summary>
	public enum FadeState
	{
		Default,
		FadeToBlack,
		FadeFromBlack
	}

	public CanvasGroup fadeCanvas; //used to fade the screen to black, and can be easily expanded to include other UI items as well if required in future.
	public FadeState fadeState; //the current fading state, see above
	public float fadeSpeed; //the speed at which to fade.


	void OnEnable()
	{
		//start listening for the 'StartFade' event, which, naturally, begins a fade either in or out
		Toolbox.Get<EventManager>().StartListening(EventName.StartFade, StartFade);
	}

	void OnDisable()
	{
		//must have a 'stoplistening' for every 'startlistening' to prevent a memory leak
		if (Toolbox.Instance != null)
			Toolbox.Get<EventManager>().StopListening(EventName.StartFade, StartFade);
	}

	void Update()
	{
		//If we are currently expecting a fade
		if (fadeState != FadeState.Default)
		{
			//basically if Fading to Black and the image's alpha is not yet 1,
			//or if Fading from Black and the images alpha is not yet 0,
			//move towards the corresponding value
			if (fadeState == FadeState.FadeToBlack && fadeCanvas.alpha < 1
			    || fadeState == FadeState.FadeFromBlack && fadeCanvas.alpha > 0)
				fadeCanvas.alpha = Mathf.MoveTowards(fadeCanvas.alpha, fadeState == FadeState.FadeFromBlack ? 0 : 1, Time.deltaTime * fadeSpeed);
			else
			{
				//sets the fade state to default when the fade is complete, then triggers an event
				//which includes a -1 if the screen has faded to black or a 1 if faded back to the screen.
				int value = fadeState == FadeState.FadeToBlack ? -1 : 1;
				fadeState = FadeState.Default;
				Toolbox.Get<EventManager>().TriggerEvent(EventName.FadeDone, value);
			}
		}
	}

	//Starts a fade in the given direction. If direction is less than 0, we fade to black. If direction
	//is greater than 0, we fade back to the screen (fade from black).
	void StartFade(int direction)
	{
		FadeState newState = direction < 0 ? FadeState.FadeToBlack : FadeState.FadeFromBlack;
		//if the newstate and the current state are both 'FadeToBlack', we fade FROM black instead
		// this is how we are cancelling a fade half way through only if the user presses the opposite direction to what they've initially pressed
		//that is, if a user presses left and then immediately right before the fade completes, the fade is 'cancelled' and we stay on the one image.
		if (newState == fadeState && fadeState == FadeState.FadeToBlack)
			fadeState = FadeState.FadeFromBlack;
		else
			fadeState = newState;
	}
}
