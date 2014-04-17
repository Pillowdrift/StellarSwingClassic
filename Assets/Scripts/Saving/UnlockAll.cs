using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
public class UnlockAll : MonoBehaviour
{
    void OnMouseDown()
    {
        Level last = Levels.AllLevels[Levels.AllLevels.Count-1];
        SaveManager.Beaten(last);
        SaveManager.Write();
    }
}
#endif