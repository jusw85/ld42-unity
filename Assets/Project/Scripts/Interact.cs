using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public string desc;
    [TextArea(3, 10)]
    public string[] examine;
    [TextArea(3, 10)]
    public string[] use;

    private int examineIdx = 0;
    private int useIdx = 0;

    private void Start () {
		
	}
	
	private void Update () {
		
	}

    public void OnMouseOver() {
        GameController.Instance.OnMouseOver(name, desc);
    }

    public void OnMouseExit() {
        GameController.Instance.OnMouseExit(name, desc);
    }

    public void OnMouseDown() {
        GameController.Instance.OnMouseDown(name, desc, examine[examineIdx], use[useIdx]);
        if (GameController.Instance.tool.Equals("eye")) {
            examineIdx = System.Math.Min(examineIdx + 1, examine.Length - 1);
        } else if (GameController.Instance.tool.Equals("hand")) {
            useIdx = System.Math.Min(useIdx + 1, use.Length - 1);
        }
    }
}
