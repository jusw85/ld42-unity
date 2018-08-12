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
    }
    public Combination[] combinations;

    private Dictionary<string, string[]> dict;

    public string Lookup(string o1, string o2, string defaultMessage) {
        string key = NormalizePair(o1, o2);
        if (dict.ContainsKey(key)) {
            return dict[key][0];
        }
        return defaultMessage;
    }

    private void Start() {
        dict = new Dictionary<string, string[]>();
        foreach (Combination c in combinations) {
            string s1 = c.o1.name;
            string s2 = c.o2.name;
            dict.Add(NormalizePair(s1, s2), c.desc);
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
