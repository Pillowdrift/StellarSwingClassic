using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutlineText
    : MonoBehaviour
{
    public Color color = Color.black;
    public int thickness = 1;

    private List<GameObject> children = new List<GameObject>();

    private GameObject prefab;

    public void Start()
    {
        // Create copy of gameobject
        prefab = (GameObject)GameObject.Instantiate(gameObject);
        prefab.SetActive(false);

        // Reset
        Reset();
    }

    private GameObject CreateText(GameObject prefab, int x, int y)
    {
        GameObject newText = (GameObject)GameObject.Instantiate(prefab);
        newText.SetActive(true);

        // Set the z back a bit so you can always see the top layer
        // (this is relative to the top layer so any negative value should work)
        newText.transform.Translate(0, 0, -1);

        newText.transform.parent = transform;

        Destroy(newText.GetComponent<OutlineText>());

        Vector2 pixelOffset = newText.guiText.pixelOffset;
        pixelOffset.x += x;
        pixelOffset.y += y;
        newText.guiText.pixelOffset = pixelOffset;

        newText.guiText.color = color;

        return newText;
    }

    public void LateUpdate()
    {
        for (int i = 0; i < children.Count; ++i)
        {
            children[i].guiText.text = guiText.text;
            children[i].guiText.enabled = guiText.enabled;
            children[i].transform.position = guiText.transform.position + new Vector3(0.0f, 0.0f, -1.0f);

            Color col = color;
            col.a = guiText.material.color.a;
            children[i].guiText.material.color = col;
        }
    }

    public void Reset()
    {
        // Destroy existing children
        for (int i = 0; i < children.Count; ++i)
        {
            Destroy(children[i]);
        }
        children.Clear();

        // Update prefab
        prefab.guiText.text = guiText.text;
        prefab.guiText.enabled = guiText.enabled;

        if (guiText != null)
        {
            int offset = 1 * thickness;

            for (int i = 1; i <= offset; ++i)
            {
                children.Add(CreateText(prefab, -i, -i));
                children.Add(CreateText(prefab, -i,  0));
                children.Add(CreateText(prefab, -i,  i));
                children.Add(CreateText(prefab,  i, -i));
                children.Add(CreateText(prefab,  i,  0));
                children.Add(CreateText(prefab,  i,  i));
                children.Add(CreateText(prefab,  0,  i));
                children.Add(CreateText(prefab,  0, -i));
            }
        }
    }
}
