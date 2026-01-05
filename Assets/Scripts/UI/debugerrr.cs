using System.Linq;
using TMPro;
using UnityEngine;

public class debugerrr : MonoBehaviour
{
    [ContextMenu("Dump Active TMP_Text (With Path)")]
    void Dump()
    {
        var texts = Resources.FindObjectsOfTypeAll<TMP_Text>()
            .Where(t => t != null && t.gameObject.scene.IsValid())
            .ToList();

        Debug.Log($"TMP_Text total in loaded scenes: {texts.Count}");

        foreach (var t in texts.Where(x => x.gameObject.activeInHierarchy && !string.IsNullOrEmpty(x.text)))
        {
            Debug.Log($"[ACTIVE] scene={t.gameObject.scene.name}, path={GetPath(t.transform)}, text={t.text}");
        }
    }

    static string GetPath(Transform tr)
    {
        string path = tr.name;
        while (tr.parent != null)
        {
            tr = tr.parent;
            path = tr.name + "/" + path;
        }
        return path;
    }
}
