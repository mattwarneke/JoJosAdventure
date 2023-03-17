using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GenericDialog : MonoBehaviour
{
    public Text title;
    public Text message;
    public Text accept, decline;
    public Button acceptButton, declineButton;

    private CanvasGroup cg;

    private void Awake()
    {
        this.cg = this.GetComponent<CanvasGroup>();
    }

    public GenericDialog OnAccept(string text, UnityAction action)
    {
        this.accept.text = text;
        this.acceptButton.onClick.RemoveAllListeners();
        this.acceptButton.onClick.AddListener(action);
        return this;
    }

    public GenericDialog OnDecline(string text, UnityAction action)
    {
        this.decline.text = text;
        this.declineButton.onClick.RemoveAllListeners();
        this.declineButton.onClick.AddListener(action);
        return this;
    }

    public GenericDialog Title(string title)
    {
        this.title.text = title;
        return this;
    }

    public GenericDialog Message(string message)
    {
        this.message.text = message;
        return this;
    }

    // show the dialog, set it's canvasGroup.alpha to 1f or tween like here
    public void Show()
    {
        this.transform.SetAsLastSibling();

        this.cg.alpha = 1f;
        this.cg.interactable = true;
        this.cg.blocksRaycasts = true;
    }

    public void Hide()
    {
        this.cg.alpha = 0f;
        this.cg.interactable = false;
        this.cg.blocksRaycasts = false;
    }

    private static GenericDialog instance;

    public static GenericDialog Instance()
    {
        if (!instance)
        {
            instance = FindObjectOfType(typeof(GenericDialog)) as GenericDialog;
            if (!instance)
                Debug.Log("There need to be at least one active GenericDialog on the scene");
        }

        return instance;
    }
}