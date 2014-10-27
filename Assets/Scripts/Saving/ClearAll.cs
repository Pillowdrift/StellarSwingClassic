using UnityEngine;
using System.Collections;
using System.IO;

#if true || UNITY_EDITOR
public class ClearAll : MonoBehaviour
{
    void OnMouseDown()
    {
		Debug.Log("Deleting " + Application.persistentDataPath);
//		Directory.Delete(Application.persistentDataPath, true);
		SaveManager.Create();
    }
}
#endif