using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSwitch : MonoBehaviour {

    private bool counterWeightOn;
    private float rotationSpeed;
    private float lastAngle;
    public float accelerationAngulaireMontee;
    public float accelerationAngulaireDescente;


    public void StartRotation()
    {
        counterWeightOn = true;
    }


    public void CancelRotation()
    {
        counterWeightOn = false;
    }

    public void Update()
    {
        
        float effectiveSpeed = 0;
        // On calcule l'angle actuel du rondin
        float angleRondin = Vector3.Angle(GetComponent<Renderer>().transform.TransformDirection(0, 0, 1), Vector3.up);
        Vector3 cross = Vector3.Cross(GetComponent<Renderer>().transform.TransformDirection(0, 0, 1), Vector3.up);
        if (cross.x < 0)
        {
            angleRondin = -angleRondin;
        }

        // Inversion de l'angle dans la scène Map Final
        angleRondin = -angleRondin;


        if (counterWeightOn)
        {
            //Si le rondin n'est pas complètement basculé
                if (angleRondin > -8f)
            {
                //On augmente sa vitesse de rotation pour simuler l'accélération
                rotationSpeed = rotationSpeed + accelerationAngulaireMontee;
                effectiveSpeed = rotationSpeed * Time.deltaTime;

            }
            else
            {
                //S'il est à sa butée, sa vitesse devient nulle
                rotationSpeed = 0;
            }
        }
        else
        {
            if (angleRondin < 22.4f)
            {
                rotationSpeed = rotationSpeed - accelerationAngulaireDescente;
                effectiveSpeed = rotationSpeed * Time.deltaTime;
            }
            else
            {
                rotationSpeed = 0;
            }
        }


        // Si l'angle est actuellement hors butée mais qu'il ne l'était pas avant, on met la vitesse à 0 pour simuler un choc dans le sol
        // absorbant l'énergie
        // Sans le précédent angle, le rondin risque de s'enfoncer dans le sol et n'en sortira que très lentement
        bool angleHorsButee = (angleRondin < -23f || angleRondin > 22.4f);
        bool lastAngleHorsButee = (lastAngle < -23f || lastAngle > 22.4f);
        if (angleHorsButee && !lastAngleHorsButee)
        {
            rotationSpeed = 0;
        }

        // On effectue la rotation à la vitesse obtenue
        transform.RotateAround(GetComponent<Renderer>().bounds.center, transform.right, effectiveSpeed);

        // On récupère l'angle de cet update pour le suivant
        lastAngle = angleRondin;




    }
}
