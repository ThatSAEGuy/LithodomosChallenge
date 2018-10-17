//Adapted from the toolbox pattern as defined at http://wiki.unity3d.com/index.php/Toolbox
//The toolbox acts as a lone singleton from which all other global classes can be accessed.
//On initialisation, the toolbox automatically finds all other Monobehaviours attached to the
//game object as components and adds them to a dictionary. From there, the user is able to
//access each of them as follows
//Toolbox.Get<MyGlobalClass>().myVariable;

//This script was originally written for 'Project Hammer', but was designed to be 
//modular and reusable, so I'm adapting it here - mostly so I can use my EventManager script
//as it was designed to be used.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : Singleton<Toolbox> 
{
	public Dictionary<System.Type, MonoBehaviour> tools;

	protected Toolbox()
	{
	}

	void Awake()
	{
		PrepareTools();
	}
	/// <summary>
	/// Called upon initialization. Finds all components on the gameObject and adds each
	/// to the dictionary of 'tools'.
	/// </summary>
	protected void PrepareTools()
	{		
		Instance.tools = new Dictionary<System.Type, MonoBehaviour>();
		MonoBehaviour[] tools = GetComponents<MonoBehaviour>();
		for (int i = 0; i < tools.Length; i++)
			AddTool(tools[i]);
	}

	/// <summary>
	/// Add a tool to the toolbox using the Monobehaviour's name as the key.
	/// </summary>
	/// <param name="tool">The monobehaviour to be added to the toolbox.</param>
	protected void AddTool(MonoBehaviour tool)
	{
		//string name = tool.GetType().ToString();
		//Debug.Log("Added singleton " + name + " to toolbox.");
		tools.Add(tool.GetType(), tool);
	}


	//We always want the string used to search our toolbox dictionary to be exactly 
	//the singleton class' name, so we go through this method to ensure this is always
	//correct
	public static T Get<T>() where T : MonoBehaviour
	{
		if (Instance != null)
			return Instance.RetrieveTool<T>(typeof(T));
		return null;
	}

	protected T RetrieveTool<T>(System.Type type) where T: MonoBehaviour
	{
		MonoBehaviour value;
		//if the requested tool is already in the toolbox, we go ahead and return that tool
		if (Instance.tools.TryGetValue(type, out value))
		{
			//If the retrieved tool matches the expected Type, we can happily return it
			if (value is T)
				return (T)value;
			//Otherwise, return nothing and report to user
			else
			{
				Debug.LogError("Something went wrong! Key + '" + type.GetType().ToString() + "' did not match the given type '" + typeof(T).ToString() + "'");
				return null;
			}

		}
		//If the tool is not in the toolbox, we create it and add it to the toolbox.
		else
		{
			T newTool = Instance.gameObject.AddComponent<T>();
			Instance.AddTool(newTool);
			Debug.LogWarning("Expected Singleton " + type.Name + " was not found. Generated it.");
			return newTool;
		}
	}
}
