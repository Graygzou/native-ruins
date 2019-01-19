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
    [SerializeField] private List<WheelItem> availableFormsList = new List<WheelItem>();
    [SerializeField] private WheelItem lockItem;

    [Header("Forms states (Read Only)")]
    [SerializeField] private bool transformationWheelOpen;

    [Header("Others fields")]
    [SerializeField] private bool transformationWheelEditorRef;
    // Usefull for general purpose.
    // Maybe use it diretly.. ?
    [SerializeField] private TransformationWheel _transformationWheel;
    public TransformationWheel TransformationWheel { get { return _transformationWheel; } }

    private Dictionary<TransformationType, TransformationForm> availableForms;
    private TransformationType currentForm = TransformationType.Human;
    private TransformationType selectedForm;

    private ParticleSystem plasmaExplosionEffect;
    private MenuManager menuManager;

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        plasmaExplosionEffect = GetComponent<ParticleSystem>();
        for (int i = 0;  i < _instance.transform.childCount; i++)
        {
            _instance.currentForm = _instance.transform.GetChild(i).gameObject.activeSelf ? (TransformationType)i : _instance.currentForm;
        }
    }

    public void OnEnable()
    {
        ResetForms();
    }

    public void Start()
    {
        menuManager = (MainManager.Instance.FindManager(MainManager.ManagerName.MenuManager) as MenuManager);
    }

    /// <summary>
    /// Create the form's dictionnary based on the current list (filled in the inspector)
    /// </summary>
    public void ResetForms()
    {
        // Construct the dictionnary based on the List
        availableForms = new Dictionary<TransformationType, TransformationForm>();
        foreach (TransformationForm form in availableFormsList)
        {
            Debug.Log(form.type);
            if (!availableForms.ContainsKey(form.type))
            {
                Debug.Log(form.type);
                availableForms.Add(form.type, form);
            }
        }
    }

    /// <summary>
    /// Method used to update the transformation wheel and take into account new forms or remove some.
    /// </summary>
    public void UpdateWheel()
    {
        ResetForms();

        Debug.Log(availableForms.Keys.Count);

        menuManager.CreateWheelIcons();
    }

    public void GenerateTransformationWheel()
    {
        

        Debug.Log(availableForms.Keys.Count);

        menuManager.CreateWheelIcons();
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

    public Dictionary<TransformationType, TransformationForm> GetAllForms()
    {
        return _instance.availableForms;
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

        // Temps arrêté
        Time.timeScale = 0f;

        // Verification des formes disponibles
        foreach (TransformationType keysForm in _instance.availableForms.Keys)
        {
            //menuManager.SetActiveIcon(_instance.availableForms[keysForm].isUnlocked);
        }

        _instance.transformationWheelOpen = true;

        // Affichage de la roue
        menuManager.DisplayTransformationWheel();
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

    #region Wheel Selection
    public void UpdateWheelSelection()
    {
        // if doesn't work: Joystick inputs
        float mouseX = Input.GetAxis("Horizontal");
        float mouseY = Input.GetAxis("Vertical");
        menuManager.UpdateWheelSelection(new Vector3(mouseX, mouseY, 0f), true, true);
    }

    public void SetSelectedForm(TransformationType type)
    {
        selectedForm = type;
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
    #endregion

    private IEnumerator ExplosionAnimation(Vector3 position)
    {
        _instance.plasmaExplosionEffect.transform.position = position;
        _instance.plasmaExplosionEffect.Play();
        yield return new WaitForSeconds(seconds: 1f);
        _instance.plasmaExplosionEffect.Stop();
    }
    
}
