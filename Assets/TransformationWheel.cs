using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used as a container. It allow to store all the wheel data.
/// </summary>
public class TransformationWheel : MonoBehaviour
{
    #region Transformation sprites
    [Header("Sprites")]
    public Sprite humanIcon;
    public Sprite bearIcon;
    public Sprite pumaIcon;
    public Sprite bearLockIcon;
    public Sprite pumaLockIcon;
    #endregion

    #region Images components
    [Header("Images Components")]
    public Image humanImage;
    public Image bearImage;
    public Image pumaImage;
    #endregion

    #region GameObjects
    [Header("GameObjects")]
    public GameObject humanSelected;
    public GameObject bearSelected;
    public GameObject pumaSelected;
    #endregion
}
