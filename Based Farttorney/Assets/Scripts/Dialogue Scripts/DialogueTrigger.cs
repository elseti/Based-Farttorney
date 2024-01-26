using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string textPath;
    public bool isRepeatable;
    private Outline _outline;

    public void PlayScript()
    {
        DialogueManager.instance.LoadDialogueList(textPath);
    }

    private void OnHoverEnter()
    {
        _outline.enabled = true;
    }

    private void OnSelectEnter()
    {
        PlayScript();
        if (!isRepeatable)
        {
            Destroy(_outline);
            Destroy(gameObject.GetComponent<DialogueTrigger>());
        }
    }
    
    private void Start()
    {
        _outline = gameObject.GetComponent<Outline>();
        if (_outline == null)
        {
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineColor = Color.yellow;
            _outline.OutlineWidth = 5f;
            _outline.enabled = false;
        }
    }
    
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && ! DialogueManager.instance.isDialoguePlaying)
        {
            if (hit.collider.gameObject == gameObject)
            {
                OnHoverEnter(); 
                if (Input.GetMouseButtonDown(0))
                {
                    OnSelectEnter();
                    _outline.enabled = false;
                }
            }
            else
            {
                _outline.enabled = false;
            }
        }
    }
    
}
