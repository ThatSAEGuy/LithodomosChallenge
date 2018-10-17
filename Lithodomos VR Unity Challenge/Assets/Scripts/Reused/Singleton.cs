//Singleton pattern retrieved from http://wiki.unity3d.com/index.php/Toolbox

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
	private static T _instance;

	private static object _lock = new object();

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
					"' already destroyed on application quit." + 
					"Won't create again. Returning null.");
				return null;
			}

			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = (T)FindObjectOfType(typeof(T));

					if (FindObjectsOfType(typeof(T)).Length > 1)
						Debug.LogError("[Singleton] Something went wrong;" +
							" there should never be more than 1 instance of a singleton!");

					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = "(Singleton) " + typeof(T).ToString();

						DontDestroyOnLoad(singleton);

						Debug.Log("[Singleton] An instance of " + typeof(T) +
							" is needed in the scene, so '" + singleton.name +
							"' was created with DontDestroyOnLoad.");
					}
				}

				return _instance;
			}
		}
	}

	private static bool applicationIsQuitting = false;

	public void OnDestroy()
	{
		applicationIsQuitting = true;
	}
}
