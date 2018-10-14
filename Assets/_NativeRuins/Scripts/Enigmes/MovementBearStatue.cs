using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBearStatue : MonoBehaviour {

    [SerializeField]
    private GameObject interactionButton;
    [SerializeField]
    private Material bearMaterial;

    private bool canBeMoved;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            bearMaterial.shader = Shader.Find("Mobile/Diffuse");
        }
    }

    public void SetCanBeMoved(bool a) {
        GetComponent<Rigidbody>().isKinematic = !a;
    }

    //public void Move(Collider other) 
    //{
    //    Vector3 DeplacementPusher = other.GetComponent<Rigidbody>().velocity;
    //    Vector3 Translation = new Vector3(DeplacementPusher.x,0,DeplacementPusher.z);
    //    Translation = transform.TransformVector(Translation);
    //    // On normalise Translation en fonction de la plus grand valeur
    //    Translation.Normalize();
    //    Translation = Translation * speed;
    //    transform.Translate(Translation, Space.World);
    //}
}
