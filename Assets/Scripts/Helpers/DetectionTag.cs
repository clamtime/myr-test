using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionTag : MonoBehaviour
{
    public bool isEnabled = false;

    public int TagCount
    {
        get { return tags.Count; }
    }

    [SerializeField]
    private List<string> tags = new List<string>();


    private void Start()
    {

    }

    public bool HasTag(string tag)
    {
        return tags.Contains(tag);
    }

    public IEnumerable<string> GetTags()
    {
        return tags;
    }

    public void RenameTag(int index, string tagName)
    {
        tags[index] = tagName;
    }

    public string GetTagAtIndex(int index)
    {
        return tags[index];
    }
}
