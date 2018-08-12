using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    public ToggleGroup toolPanel;
    public Text actionText;
    public GameObject textPanel;
    public Text descriptionText;
    public Combine combinations;

    [System.NonSerialized]
    public string tool;
    private bool textPanelOpen = false;
    private bool isCombining = false;
    private string combineText;
    private string obj1_id;
    private string obj1_desc;

    private void Start() {
        UpdateSelectedTool();
        UpdateActionText("");
        textPanel.SetActive(false);
    }

    public void OnToolChanged() {
        UpdateSelectedTool();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (textPanelOpen) {
                textPanel.SetActive(false);
                textPanelOpen = false;
            }
        }
    }

    private void UpdateSelectedTool() {
        foreach (Toggle t in toolPanel.ActiveToggles()) {
            if (t.isOn) {
                tool = t.name.ToLower();
                Debug.Log("Tool selected: " + tool);
            }
        }
    }

    public void OnMouseOver(string id, string desc) {
        if (textPanelOpen)
            return;
        string text = "";
        if (tool.Equals("eye")) {
            text = "Examine " + desc;
        } else if (tool.Equals("hand")) {
            text = "Use " + desc;
        } else if (tool.Equals("combine")) {
            if (isCombining) {
                text = combineText + desc;
            } else {
                text = "Use " + desc + " with ";
            }
        }
        UpdateActionText(text);
    }

    public void OnMouseExit(string id, string desc) {
        if (textPanelOpen)
            return;
        string text = "";
        if (tool.Equals("combine") && isCombining) {
            text = combineText;
        }
        UpdateActionText(text);
    }

    public void OnMouseDown(string id, string desc, string examine, string use) {
        if (textPanelOpen)
            return;
        string text = "";
        if (tool.Equals("eye")) {
            text = examine;
        } else if (tool.Equals("hand")) {
            text = use;
        } else if (tool.Equals("combine")) {
            if (isCombining) {
                text = combinations.Lookup(obj1_id, id, obj1_desc, desc);
                isCombining = false;
                combineText = "";
                obj1_id = "";
                obj1_desc = "";
            } else {
                isCombining = true;
                combineText = actionText.text;
                obj1_id = id;
                obj1_desc = desc;
                return;
            }
        }
        UpdateActionText("");
        textPanelOpen = true;
        StartCoroutine(ActivateToolPanel(text));
    }

    private IEnumerator ActivateToolPanel(string text) {
        yield return new WaitForEndOfFrame();
        UpdateActionText("");
        textPanelOpen = true;
        UpdateDescriptionText(text);
        textPanel.SetActive(true);
        yield return null;
    }

    private void UpdateActionText(string text) {
        actionText.text = text;
    }

    private void UpdateDescriptionText(string text) {
        descriptionText.text = text;
    }
}
