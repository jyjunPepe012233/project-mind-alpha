using MinD.Utility;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MinD.Runtime {

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;

    public static T Instance {
        get {
            if (instance == null) {

                instance = FindObjectOfType<T>();

                if (instance == null) {
                    GameObject newSingleton = new GameObject(typeof(T).Name, typeof(T));
                    instance = newSingleton.GetComponent<T>();

                    if (instance.transform != instance.transform.root) {
                        DontDestroyOnLoad(instance.transform.root);
                    } else {
                        DontDestroyOnLoad(instance);
                    }

                }

            } // 싱글톤. instance가 할당되어있지 않을 때 instance를 검색한다.
            // instance를 찾지 못하면 새로운 instance를 만들어 할당한다.

            return instance;
        }
    }

    protected void Awake() {
        if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    

    protected virtual void OnSceneChanged(Scene oldScene, Scene newScene) {
    }
}

}