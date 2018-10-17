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

    public enum TransformationType : int
    {
        Human = 0,
        Bear = 1,
        Puma = 2
    }

    [SerializeField]
    private GameObject[] availableForms;
    [SerializeField]
    private GameObject transformationWheel;
    [SerializeField]
    private bool transformationWheelOpen;

    private ParticleSystem plasmaExplosionEffect;
    private bool pumaUnlocked;
    private bool bearUnlocked;
    private TransformationType currentForm = TransformationType.Human;
    private TransformationType selectedForm;

    private Color colorStartHuman;

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
        _instance.pumaUnlocked = false;
        _instance.bearUnlocked = true;
        _instance.transformationWheel.SetActive(false);
        for (int i = 0;  i < _instance.transform.childCount; i++)
        {
            _instance.currentForm = _instance.transform.GetChild(i).gameObject.activeSelf ? (TransformationType)i : _instance.currentForm;
        }
        Debug.Log(_instance.currentForm);
    }

    public int IsPumaUnlocked()
    {
        return System.Convert.ToInt32(_instance.pumaUnlocked == true);
    }

    public int IsBearUnlocked()
    {
        return System.Convert.ToInt32(_instance.bearUnlocked == true);
    }

    public bool IsTransformationWheelOpened()
    {
        return transformationWheelOpen;
    }

    public void SetPumaUnlocked(bool puma)
    {
        pumaUnlocked = puma;
    }

    public void SetBearUnlocked(bool ours)
    {
        bearUnlocked = ours;
    }

    public int GetCurrentForm()
    {
        return System.Convert.ToInt32(currentForm);
    }

    public void OpenTransformationWheel()
    {
        if (!(_instance.availableForms[(int)TransformationType.Human].GetComponent<MovementController>().isDeath()))
        {
            // Unsubscribe the player movement and plug it to the transformation wheel
            
            
            _instance.transformationWheelOpen = true;
            // Verification des formes disponibles
            if (!_instance.pumaUnlocked)
            {
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconPuma").SetActive(false);
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconPumaLocked").SetActive(true);
            }
            else
            {
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconPuma").SetActive(true);
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconPumaLocked").SetActive(false);
            }

            if (!bearUnlocked)
            {
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconBear").SetActive(false);
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconBearLocked").SetActive(true);
            }

            // Temps arrêté
            Time.timeScale = 0f;

            // Affichage de la roue
            _instance.transformationWheel.SetActive(true);

            // Données utiles à la sélection
            Vector3 centreScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 positionMouse = Input.mousePosition;
            Vector3 difference = positionMouse - centreScreen;

            // Si en dehors du centre de la roue :
            if (difference.magnitude > 125)
            {
                // Si sur le tiers du dessus :
                // coefficient directeur de la droite "gauche"
                float a1 = -182f / 312f;
                // "ordonnée à l'origine"
                float b1 = centreScreen.y - a1 * centreScreen.x;
                // coefficient directeur de la droite "droite"
                float a2 = -a1;
                // "ordonnée à l'origine"
                float b2 = centreScreen.y - a2 * centreScreen.x;

                // SELECTION HUMAIN
                if ((positionMouse.y > positionMouse.x * a1 + b1) && (positionMouse.y > positionMouse.x * a2 + b2))
                {
                    _instance.selectedForm = TransformationType.Human;
                    GameObject.Find("Affichages/TransformationSystem/Wheel/IconHumanSelected").SetActive(true);
                }
                else
                {
                    GameObject.Find("Affichages/TransformationSystem/Wheel/IconHumanSelected").SetActive(false);
                }

                // SELECTION OURS
                if ((positionMouse.y < positionMouse.x * a2 + b2) && (positionMouse.x > centreScreen.x) && bearUnlocked)
                {
                    _instance.selectedForm = TransformationType.Bear;
                    GameObject.Find("Affichages/TransformationSystem/Wheel/IconBearSelected").SetActive(true);
                }
                else
                {
                    GameObject.Find("Affichages/TransformationSystem/Wheel/IconBearSelected").SetActive(false);
                }

                // SELECTION PUMA
                if ((positionMouse.y < positionMouse.x * a1 + b1) && (positionMouse.x < centreScreen.x) && pumaUnlocked)
                {
                    _instance.selectedForm = TransformationType.Puma;
                    GameObject.Find("Affichages/TransformationSystem/Wheel/IconPumaSelected").SetActive(true);
                }
                else
                {
                    GameObject.Find("Affichages/TransformationSystem/Wheel/IconPumaSelected").SetActive(false);
                }
            }
            else
            {
                _instance.selectedForm = _instance.currentForm;
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconHumanSelected").SetActive(false);
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconPumaSelected").SetActive(false);
                GameObject.Find("Affichages/TransformationSystem/Wheel/IconBearSelected").SetActive(false);
            }
        }
    }

    public void CloseTransformationWheel()
    {
        Debug.Log("Close");
        _instance.transformationWheelOpen = false;
        Time.timeScale = 1;
        _instance.transformationWheel.SetActive(false);
        if (selectedForm != currentForm)
        {
            Transformation();
        }
    }

    private void Transformation()
    {
        // Memorisation position et orientation actuelle
        Vector3 positionCourant = new Vector3();
        // Desactiver toutes les formes
        foreach (GameObject transformation in _instance.availableForms)
        {
            positionCourant = transformation.activeSelf ? transformation.transform.position : positionCourant;
            transformation.SetActive(false);
        }
        if (_instance.currentForm != _instance.selectedForm)
        {
            StartCoroutine(ExplosionAnimation(positionCourant));
        }
        // Activation nouvelle forme
        _instance.availableForms[(int)selectedForm].transform.position = positionCourant;
        _instance.availableForms[(int)selectedForm].SetActive(true);
        // Override the inputs of the current forms
        _instance.availableForms[(int)selectedForm].GetComponent<MovementController>().RegisterInputs();
        _instance.currentForm = _instance.selectedForm;
    }

    public void Transformation(TransformationType type)
    {
        if(_instance.currentForm != type)
        {
            _instance.selectedForm = type;
            Transformation();
        }
    }

    private IEnumerator ExplosionAnimation(Vector3 position)
    {
        _instance.plasmaExplosionEffect.transform.position = position;
        _instance.plasmaExplosionEffect.Play();
        yield return new WaitForSeconds(seconds: 1f);
        _instance.plasmaExplosionEffect.Stop();
    }

}
