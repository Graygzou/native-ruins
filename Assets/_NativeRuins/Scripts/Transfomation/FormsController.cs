using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FormsController : MonoBehaviour
{
    #region Singleton
    private static FormsController _instance;

    public static FormsController Instance { get { return _instance; } }
    #endregion

    [Header("Transformations")]
    [SerializeField]
    private Dictionary<TransformationType, TransformationForm> availableForms = new Dictionary<TransformationType, TransformationForm>();

    [Header("Forms states (Read Only)")]
    [SerializeField]
    private bool transformationWheelOpen;

    private ParticleSystem plasmaExplosionEffect;
    
    private TransformationType currentForm = TransformationType.Human;
    private TransformationType selectedForm;

    private Color colorStartHuman;

    //private InGameUI inGameUI;
    private MenuManager menuManager;

    // Use this for initialization
    protected void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        //ColorStartHuman = HumanForm.GetComponentInChildren<Renderer>().material.color;
        plasmaExplosionEffect = GetComponent<ParticleSystem>();

        for (int i = 0;  i < _instance.transform.childCount; i++)
        {
            _instance.currentForm = _instance.transform.GetChild(i).gameObject.activeSelf ? (TransformationType)i : _instance.currentForm;
        }
        Debug.Log(_instance.currentForm);

        //inGameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
    }

    public void Start()
    {
        menuManager = (MainManager.Instance.FindManager(MainManager.ManagerName.MenuManager) as MenuManager);
    }

    public int IsFormUnlocked(TransformationType type)
    {
        return System.Convert.ToInt32(_instance.availableForms[type].isUnlocked);
    }

    public bool IsTransformationWheelOpened()
    {
        return transformationWheelOpen;
    }

    public Dictionary<TransformationType, TransformationForm>.KeyCollection GetAvailableForms()
    {
        return _instance.availableForms.Keys;
    }

    public void SetFormState(TransformationType type, bool state)
    {
        TransformationForm form = _instance.availableForms[type];
        form.isUnlocked = state;
        _instance.availableForms[type] = form;
    }

    public int GetCurrentForm()
    {
        return System.Convert.ToInt32(currentForm);
    }

    public void OpenTransformationWheel()
    {
        // Override camera movement event to plug in transformation wheel events
        InputManager.SubscribeMouseMovementsChangedEvents(InputManager.ActionsLabels.Movement, new string[] { "Horizontal", "Vertical" }, new System.Action[] { UpdateWheelSelection, UpdateWheelSelection });

        _instance.transformationWheelOpen = true;

        // Temps arrêté
        Time.timeScale = 0f;

        // Verification des formes disponibles
        foreach(TransformationType keysForm in _instance.availableForms.Keys)
        {
            menuManager.SetActiveBearIcon(_instance.availableForms[keysForm].isUnlocked);
        }

        // Affichage de la roue
        menuManager.DisplayTransformationWheel();
    }

    public void UpdateWheelSelection()
    {
        // Test with the mouse inputs.
        //if(!menuManager.UpdateWheelSelectionMouse(Input.mousePosition, bearUnlocked, pumaUnlocked))
        //{
            // if doesn't work: Joystick inputs
            float mouseX = Input.GetAxis("Horizontal");
            float mouseY = Input.GetAxis("Vertical");
            menuManager.UpdateWheelSelection(new Vector3(mouseX, mouseY, 0f));
        //}
    }

    public void SetSelectedForm(TransformationType type)
    {
        selectedForm = type;
    }

    public void CloseTransformationWheel()
    {
        Debug.Log("Close");
        _instance.transformationWheelOpen = false;
        Time.timeScale = 1;

        (MainManager.Instance.FindManager(MainManager.ManagerName.MenuManager) as MenuManager).CloseTransformationWheel();

        if (selectedForm != currentForm)
        {
            Transformation();

            // No need to Unsusbcribe transformation's event because the Transformation method call RegisterInputs()
        }
        else
        {
            // Subscribe back the movement events.
            InputManager.UnsubscribeMouseMovementsChangedEvent(InputManager.ActionsLabels.Movement);
            //_instance.availableForms[(int)selectedForm].GetComponent<MovementController>().RegisterPlayerMovementsInputs();
        }
    }

    public void Transformation(TransformationType type)
    {
        if (_instance.currentForm != type && type != TransformationType.None)
        {
            _instance.selectedForm = type;
            Transformation();
        }
    }

    private void Transformation()
    {
        // Memorisation position et orientation actuelle
        Vector3 positionCourant = new Vector3();
        // Desactiver toutes les formes
        /*
        foreach (GameObject transformation in _instance.availableForms)
        {
            positionCourant = transformation.activeSelf ? transformation.transform.position : positionCourant;
            transformation.SetActive(false);
            transformation.GetComponent<MovementController>().enabled = false;
        }
        if (_instance.currentForm != _instance.selectedForm)
        {
            StartCoroutine(ExplosionAnimation(positionCourant));
        }

        // Activation nouvelle forme
        /*
        _instance.availableForms[(int)selectedForm].transform.position = positionCourant;
        _instance.availableForms[(int)selectedForm].SetActive(true);
        _instance.availableForms[(int)selectedForm].GetComponent<MovementController>().enabled = true;

        // Override the inputs of the current forms
        _instance.availableForms[(int)selectedForm].GetComponent<MovementController>().RegisterInputs();
        _instance.currentForm = _instance.selectedForm;*/
    }

    private IEnumerator ExplosionAnimation(Vector3 position)
    {
        _instance.plasmaExplosionEffect.transform.position = position;
        _instance.plasmaExplosionEffect.Play();
        yield return new WaitForSeconds(seconds: 1f);
        _instance.plasmaExplosionEffect.Stop();
    }

}
