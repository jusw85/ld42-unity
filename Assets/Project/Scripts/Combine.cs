using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : MonoBehaviour {
    [System.Serializable]
    public struct Combination {
        public Interact o1;
        public Interact o2;
        [TextArea(3, 10)]
        public string[] desc;
        [System.NonSerialized]
        public int idx;
        public GameObject[] toDisable;
        public GameObject[] toEnable;
    }
    public Combination[] combinations;

    private Dictionary<string, Combination> dict;

    public bool CanCombine(string o1, string o2) {
        string key = NormalizePair(o1, o2);
        return dict.ContainsKey(key);
    }

    public string CombineObjects(string o1, string o2) {
        string key = NormalizePair(o1, o2);
        Combination c = dict[key];
        string text = c.desc[c.idx++];
        if (c.idx > c.desc.Length - 1)
            c.idx = c.desc.Length - 1;
        dict[key] = c;
        foreach (GameObject obj in c.toDisable) {
            obj.SetActive(false);
        }
        foreach (GameObject obj in c.toEnable) {
            obj.SetActive(true);
        }
        return text;
    }

    private void Start() {
        dict = new Dictionary<string, Combination>();
        foreach (Combination c in combinations) {
            string s1 = c.o1.name;
            string s2 = c.o2.name;
            dict.Add(NormalizePair(s1, s2), c);
        }
    }

    private string NormalizePair(string s1, string s2) {
        s1 = s1.ToLower();
        s2 = s2.ToLower();
        if (s1.CompareTo(s2) > 0) {
            string tmp = s1;
            s1 = s2;
            s2 = tmp;
        }
        return s1 + "," + s2;
    }

    private void Update() {

    }
}
