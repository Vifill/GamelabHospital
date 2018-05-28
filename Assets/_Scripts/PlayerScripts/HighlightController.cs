using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class HighlightController : MonoBehaviour 
{
    public bool ItemTextActive;
    public GameObject ItemText;
    public Shader StandardOutlineShader;
    public Shader WebGLOutlineShader;
    [System.NonSerialized]
    public Shader HighlightShader;
    public static GameObject HighlightedObject;

    private RectTransform rect;
    private Actionable PreviousActionable;
    private ToolController ToolCtrl;
    private Transform PlayerTransform;
    private ActionableActioner Actioner;

    private List<GameObject> ItemTexts = new List<GameObject>();

    private void Awake()
    {
#if UNITY_EDITOR
        print("<color=green>run in Editor</color>");
        HighlightShader = StandardOutlineShader;
#elif UNITY_WEBGL
        print("<color=green>run in webGL</color>");
        HighlightShader = WebGLOutlineShader;
#elif UNITY_STANDALONE
        print("<color=green>run in StandAlone</color>");
        HighlightShader = StandardOutlineShader;
#else
        print("<color=green>run in other</color>");
        HighlightShader = StandardOutlineShader;
#endif
    }

    private void Start() 
	{
        PlayerTransform = FindObjectOfType<PlayerActionController>()?.transform;
        ToolCtrl = PlayerTransform?.GetComponent<ToolController>();
        rect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        Actioner = PlayerTransform?.GetComponent<ActionableActioner>();
    }
	
	// Update is called once per frame
	private void Update() 
	{
        if (PlayerTransform != null && ToolCtrl != null)
        {
            //var actionable = GetActionablesUtility.GetActionableForHighlight(ToolCtrl, PlayerTransform)?.GetMostRelevantAction(ToolCtrl.GetCurrentToolName(), PlayerTransform.gameObject);
            var actionable = GetActionablesUtility.GetActionableForHighlight(ToolCtrl, PlayerTransform);

            if (actionable == null)
            {
                HighlightedObject = null;
                PreviousActionable?.RemoveHighlight();
                RemoveTexts();
            }

            else 
            {
                if (PreviousActionable != null)
                {
                    PreviousActionable.RemoveHighlight();
                    RemoveTexts();
                }

                if (!actionable.IsHighlighted)
                {
                    if (actionable.CanBeActioned(ToolCtrl.GetCurrentToolName(), PlayerTransform.gameObject) || actionable == Actioner.CurrentAction)
                    {
                        actionable.SetHighlight(HighlightShader);
                    }
                    else
                    {
                        //cant be used... RED
                        actionable.SetHighlight(HighlightShader, new Color(0.8f, 0, 0));
                    }
                }
                
               

                HighlightedObject = actionable.gameObject;

                AddText(actionable);
            }
            //if (actionable != null)
            //{
            //    actionable.SetHighlight(HighlightShader);
            //    HighlightedObject = actionable.gameObject;
            //}

            PreviousActionable = actionable;
        }
    }

    void RemoveTexts()
    {
        for (int i = 0; i < ItemTexts.Count; i++)
        {
            GameObject tempGO = ItemTexts[i];
            ItemTexts.Remove(tempGO);
            Destroy(tempGO);
        }
    }

    void AddText(Actionable pActionable)
    {
        if (!ItemTextActive)
        {
            return;
        }

        GameObject tempTextGO = Instantiate(ItemText, rect.transform);

        Text tempText = tempTextGO.GetComponent<Text>();
        string itemName = pActionable.name;
        string[] splitName = itemName.Split('(');
        tempText.text = splitName[0];
        tempText.rectTransform.anchoredPosition = WorldToCanvas(pActionable.transform.position);

        ItemTexts.Add(tempTextGO);
    }

    Vector2 WorldToCanvas(Vector3 pVector)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pVector);
        return new Vector2(((ViewportPosition.x * rect.sizeDelta.x) - (rect.sizeDelta.x * 0.5f)),
                           ((ViewportPosition.y * rect.sizeDelta.y) - (rect.sizeDelta.y * 0.5f)));
    }
}
