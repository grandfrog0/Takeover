using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorKit : MonoBehaviour
{
    private void Update()
    {
# if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
#endif
    }
    public static T DebugIt<T>(T obj)
    {
        Debug.Log(obj);
        return obj;
    }
}
