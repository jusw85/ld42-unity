using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Fungus;

public class GameController : Singleton<GameController> {

    public ToggleGroup toolPanel;
    public Text actionText;
    public Combine combinations;
    public Flowchart flowChart;

    public GameObject[] endObjs;
    public Button endButton;

    [System.NonSerialized]
    public string tool;
    private bool isCombining = false;
    private string combineText;
    private string obj1_id;
    private string obj1_desc;

    private void Start() {
        UpdateSelectedTool();
        UpdateActionText("");
    }

    public void OnToolChanged() {
        UpdateSelectedTool();
    }

    private void Update() {
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
        if (isTextPanelOpen())
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
        if (isTextPanelOpen())
            return;
        string text = "";
        if (tool.Equals("combine") && isCombining) {
            text = combineText;
        }
        UpdateActionText(text);
    }

    public bool OnMouseDown(string id, string desc, string examine, string use) {
        if (isTextPanelOpen())
            return false;
        flowChart.ExecuteBlock("PlaySound");
        string text = "";
        if (tool.Equals("eye")) {
            text = examine;
        } else if (tool.Equals("hand")) {
            text = use;
        } else if (tool.Equals("combine")) {
            if (isCombining) {
                if (!combinations.CanCombine(obj1_id, id)) {
                    text = "You can't seem to use the " + obj1_desc + " with the " + desc + ".";
                } else {
                    text = combinations.CombineObjects(obj1_id, id);
                }
                isCombining = false;
                combineText = "";
                obj1_id = "";
                obj1_desc = "";
            } else {
                isCombining = true;
                combineText = actionText.text;
                obj1_id = id;
                obj1_desc = desc;
                return false;
            }
        }
        UpdateActionText("");
        if (canEnd()) { // To trigger on end condition
            endButton.gameObject.SetActive(true);
        }
        flowChart.SetStringVariable("msg", text);
        flowChart.ExecuteBlock("DialogBox");
        return true;
    }

    public void EndGame() {
        if (!isBlockExecuting("Ending")) {
            flowChart.SetBooleanVariable("toEnd", true);
            flowChart.ExecuteBlock("Ending");
        }
    }

    private bool isTextPanelOpen() {
        return isBlockExecuting("DialogBox");
    }

    private bool isBlockExecuting(string blockName) {
        foreach (Block b in flowChart.GetExecutingBlocks()) {
            if (b.BlockName.Equals(blockName)) {
                return true;
            }
        }
        return false;
    }

    private bool canEnd() {
        if (endObjs == null) {
            return true;
        }
        foreach (GameObject obj in endObjs) {
            if (!obj.activeSelf) {
                return false;
            }
        }
        return true;
    }

    private void UpdateActionText(string text) {
        actionText.text = text;
    }

}
