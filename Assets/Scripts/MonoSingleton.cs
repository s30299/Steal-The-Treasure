using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Szukanie instancji w scenie, jeśli nie została jeszcze przypisana
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T)} is missing from the scene!");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            // Opcjonalnie: DontDestroyOnLoad(gameObject); // Zachowanie między scenami
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Usuń duplikaty
        }
    }
}