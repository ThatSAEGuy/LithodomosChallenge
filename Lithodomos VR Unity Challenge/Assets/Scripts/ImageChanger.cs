using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageChanger : MonoBehaviour 
{
	public string[] imagePaths; //a string array containing the paths to our 360 images
	public int imageIncrement; //whether the user last pressed left (-1) or right (1)
	public int currentImage; //the position in the array of the currently viewed image, will be a value between 0 and imagePaths.length
	private int previousImage; //the position in the array of the previous image
	public Material sphereMat; //a reference to the viewing sphere's material
	public Texture storedTexture; //stores the previously viewed image for quick loading when returning to last image

	void OnEnable()
	{
		//Start listening for the 'fade done' event, which triggers switching the image
		Toolbox.Get<EventManager>().StartListening(EventName.FadeDone, FadeDone);
	}

	void OnDisable()
	{
		//must always have a stoplistening call for every startlistening
		if (Toolbox.Instance != null)
			Toolbox.Get<EventManager>().StopListening(EventName.FadeDone, FadeDone);
	}

	void Update () 
	{
		//maps the onscreen arrows to the arrow keys for PC debugging convenience
		if (imagePaths.Length > 0)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				ButtonPress(-1);
			if (Input.GetKeyDown(KeyCode.RightArrow))
				ButtonPress(1);
		}
	}

	/// <summary>
	/// When called, triggers a fade transition event and records the direction pressed (passed in as 'value', left should be -1 and right should be 1)
	/// </summary>
	/// <param name="value">The direction to move through the array in. Left should be -1 annd right should 1.</param>
	public void ButtonPress(int value)
	{
		if (value != imageIncrement)
		{
			Toolbox.Get<EventManager>().TriggerEvent(EventName.StartFade, -1);
			imageIncrement = value;
		}
	}

	/// <summary>
	/// Called when the "FadeDone" event is triggered. Initiates the actual image load & change.
	/// </summary>
	/// <param name="fadeState">The 'FadeDone' event should pass either -1 or 1. This indicates whether the fade was ToBlack (1) or FromBlack (-1).</param>
	private void FadeDone(int fadeState)
	{
		//We only want to change the image if the screen has just faded to black, which will mean fadeState has a value of 1.
		if (fadeState < 0)
			ChangeImage(imageIncrement);
	}

	/// <summary>
	/// Changes the image in a given direction.
	/// </summary>
	/// <param name="direction">The direction in which the image cycles through the array. If not given as -1 or 1, it takes the Sign of the value.</param>
	private void ChangeImage(int direction)
	{
		int prev = currentImage;
		//ensure we are only incrementing by 1
		if (Mathf.Abs(direction) > 1)
			direction = (int)Mathf.Sign(direction);
		//Loops the image around the length of the imagePaths array, eg. 0-1 becomes length - 1.
		currentImage = currentImage.Loop(direction, imagePaths.Length);
		//If we are returning to the previous image, we load it from memory rather than disk.
		if (previousImage == currentImage)
		{
			SetPreviousTexture();
			Toolbox.Get<EventManager>().TriggerEvent(EventName.StartFade, 1);
		}
		//if the texture loads correctly, fade the screen back in from black
		else if (LoadTexture(imagePaths[currentImage]))
			Toolbox.Get<EventManager>().TriggerEvent(EventName.StartFade, 1);

		imageIncrement = 0;
		previousImage = prev;
	}

	/// <summary>
	/// Loads a texture and applies it to the viewing sphere. Also stores the current texture in case the user returns to it, for faster loading.
	/// </summary>
	/// <returns><c>true</c>, if texture was successfully loaded, <c>false</c> otherwise.</returns>
	/// <param name="texturePath">Path of the texture to be loaded.</param>
	private bool LoadTexture(string texturePath)
	{
		Texture newTexture = Resources.Load<Texture>(texturePath);
		//if the texture was loaded correctly, set the material's texture
		if (newTexture)
		{
			Resources.UnloadAsset(storedTexture);
			storedTexture = sphereMat.mainTexture;
			sphereMat.SetTexture("_MainTex", newTexture);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Sets the viewing sphere's texture to the previous texture, allowing for faster loading times. Called when the user returns to the previously viewed image.
	/// </summary>
	private void SetPreviousTexture()
	{
		Texture currentTexture = sphereMat.mainTexture;
		sphereMat.SetTexture("_MainTex", storedTexture);
		storedTexture = currentTexture;
	}

	#if UNITY_EDITOR
	void OnApplicationQuit()
	{
		//Resets the display to the first image on quit.
		//Just for use in the Editor.
		LoadTexture(imagePaths[0]);
	}
	#endif
}
