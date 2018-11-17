using UnityEngine;
using System.Collections;

namespace DigitalRuby.BowAndArrow
{
    [RequireComponent(typeof(AudioSource))]
    public class BowScript : MonoBehaviour
    {
        [Header("Bow Structure")]
        [Tooltip("Bow shaft game object. This is the object that should be rotated and moved to change the bow position in the world.")]
        public GameObject BowShaft;

        [Tooltip("Bow string game object. Do not modify the transform of this object.")]
        public GameObject BowString;

        [Tooltip("Top anchor of the bow, usually attached to the top end of the bow.")]
        public GameObject TopAnchor;

        [Tooltip("Bottom anchor of the bow, usually attached to the bottom end of the bow.")]
        public GameObject BottomAnchor;

        [Tooltip("Area where the draw must start from. The bow will not start drawing an arrow unless the click happens in this area.")]
        public Collider2D DrawStartArea;

        [Tooltip("Area where the draw must stay in. If the click moves outside this area, the draw fizzles.")]
        public Collider2D DrawTotalArea;

        [Tooltip("Used to determine the minimum draw strength of the bow string.")]
        public GameObject MinDrawAnchor;

        [Tooltip("Used to determine the maximum draw strength of the bow string. " +
            "Distance from this to MinDrawAnchor is compared against how far they actually drew back the bow string to calculate final arrow velocity.")]
        public GameObject MaxDrawAnchor;

        [Header("Bow Firing")]
        [Tooltip("Arrow object to clone when firing.")]
        public GameObject Arrow;

        [Tooltip("Allow the bow to rotate this much in radians (+/-) from the bow start angle. 90 degrees would be about 1.6.")]
        public float MaxRotationAngleRadians = 0.6f;

        [Tooltip("How long (in seconds) before the bow can be fired again.")]
        public float Cooldown = 0.5f;

        [Tooltip("Whether the bow can misfire if it is drawn to far back or forward.")]
        public bool AllowFizzling = true;

        [Tooltip("Max speed if the shot fizzles (i.e. they pulled it too far backward or forward).")]
        public float FizzleSpeed = 10.0f;

        [Tooltip("Base speed at which arrows leave the bow. Will be lower if the bow is not pulled back all the way.")]
        public float FireSpeed = 80.0f;

        [Header("Bow Sounds")]
        [Tooltip("Sounds for knocking arrows")]
        public AudioClip[] KnockClips;

        [Tooltip("Sounds for drawing the bow")]
        public AudioClip[] DrawClips;

        [Tooltip("Sounds for firing the arrow")]
        public AudioClip[] FireClips;

        private LineRenderer bowStringLineRenderer1;
        private LineRenderer bowStringLineRenderer2;
        private AudioSource audioSource;
        private bool drawingBow;
        private GameObject currentArrow;
        private float cooldownTimer;
        private float startAngle;

        private float DifferenceBetweenAngles(float angle1, float angle2)
        {
            float angle = angle1 - angle2;
            //return ((angle + (float)Math.PI) % ((float)Math.PI * 2.0f)) - (float)Math.PI;
            return Mathf.Atan2(Mathf.Sin(angle), Mathf.Cos(angle));
        }

        private void RenderBowString(Vector3 arrowPos)
        {
            Vector3 startPoint = TopAnchor.transform.position;
            Vector3 endPoint = BottomAnchor.transform.position;

            if (drawingBow)
            {
                bowStringLineRenderer2.gameObject.SetActive(true);
                bowStringLineRenderer1.SetPosition(0, startPoint);
                bowStringLineRenderer1.SetPosition(1, arrowPos);
                bowStringLineRenderer2.SetPosition(0, arrowPos);
                bowStringLineRenderer2.SetPosition(1, endPoint);
            }
            else
            {
                bowStringLineRenderer2.gameObject.SetActive(false);
                bowStringLineRenderer1.SetPosition(0, startPoint);
                bowStringLineRenderer1.SetPosition(1, endPoint);
            }
        }

        private void PlayRandomSound(AudioClip[] clips)
        {
            if (clips != null && clips.Length != 0)
            {
                audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            }
        }

        private Vector3 GetArrowPositionForDraw()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0.0f;

            // if they are drawing the bow, but it's too far forward, set the draw position to some minimum position backwards
            Vector3 dirFacing = (MaxDrawAnchor.transform.position - MinDrawAnchor.transform.position).normalized;
            float angleFacing = Mathf.Atan2(MaxDrawAnchor.transform.position.y - MinDrawAnchor.transform.position.y, MaxDrawAnchor.transform.position.x - MinDrawAnchor.transform.position.x);
            float angleClicking = Mathf.Atan2(worldPos.y - MinDrawAnchor.transform.position.y, worldPos.x - MinDrawAnchor.transform.position.x);
            float angleDiff = Mathf.Abs(DifferenceBetweenAngles(angleFacing, angleClicking));
            if (angleDiff >= (Mathf.PI * 0.5f))
            {
                worldPos = MinDrawAnchor.transform.position + (dirFacing * 0.01f);
            }
            else if (!AllowFizzling)
            {
                // if they've drawn the bow too far back, force it to be at the max distance
                float maxDistance = Vector3.Distance(MaxDrawAnchor.transform.position, MinDrawAnchor.transform.position);
                float actualDistance = Vector3.Distance(worldPos, MinDrawAnchor.transform.position);
                if (actualDistance > maxDistance)
                {
                    Vector3 dirClicking = (worldPos - MinDrawAnchor.transform.position).normalized;
                    worldPos = MinDrawAnchor.transform.position + (dirClicking * maxDistance);
                }
            }

            return worldPos;
        }

        private void BeginBowDraw()
        {
            // start drawing back the bow

            // play a knock arrow sound
            PlayRandomSound(KnockClips);

            // create a new arrow that will be fired later
            currentArrow = GameObject.Instantiate(Arrow);
            currentArrow.transform.rotation = BowShaft.transform.rotation;
            currentArrow.SetActive(true);

            // find where the arrow should be based on where they started drawing back the bow
            Vector3 pos = GetArrowPositionForDraw();
            currentArrow.transform.position = pos;
            drawingBow = true;
        }

        private void ContinueBowDraw()
        {
            // if we are still in the good area for drawing back the bow, continue drawing the bow back
            if (!AllowFizzling || DrawTotalArea.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                Vector3 pos = GetArrowPositionForDraw();
                if (MaxRotationAngleRadians > 0.0f)
                {
                    float angle = Mathf.Atan2(pos.y - BowShaft.transform.position.y, pos.x - BowShaft.transform.position.x);
                    float angleDiff = Mathf.Abs(DifferenceBetweenAngles(angle, startAngle));
                    if (angleDiff <= MaxRotationAngleRadians)
                    {
                        // allow rotation of the bow
                        BowShaft.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
                    }
                }

                // set that arrow position and rotation based on how far the bow is drawn back
                currentArrow.GetComponent<Rigidbody2D>().MovePosition(pos);
                currentArrow.GetComponent<Rigidbody2D>().MoveRotation(BowShaft.transform.rotation.eulerAngles.z);
                RenderBowString(pos);
            }
            else
            {
                // fizzle
                FireArrow(true);
            }
        }

        private void FireArrow(bool fizzle)
        {
            // play fire sound
            PlayRandomSound(FireClips);
            float speed;
            cooldownTimer = Cooldown;

            if (fizzle)
            {
                // fizzle, fire at random fizzle speed
                speed = Random.Range(-FizzleSpeed, FizzleSpeed);
            }
            else
            {
                // success, fire based on how far the bow was drawn back and how centered the arrow was
                Vector3 pos = GetArrowPositionForDraw();
                float baseDistance = Vector3.Distance(MaxDrawAnchor.transform.position, MinDrawAnchor.transform.position);
                float clickDistance = Vector3.Distance(MinDrawAnchor.transform.position, pos);

                // give boost on speed
                float speedBoost = clickDistance / baseDistance;

                // penalize boost as angle from center increases, i.e. they haven't held the arrow in the direct center of the bow
                float angleFromCenter = Mathf.Rad2Deg * Mathf.Atan2(pos.y - MinDrawAnchor.transform.position.y, pos.x - MinDrawAnchor.transform.position.x);
                angleFromCenter -= BowShaft.transform.rotation.eulerAngles.z;
                angleFromCenter *= Mathf.Deg2Rad;
                angleFromCenter = Mathf.Abs(DifferenceBetweenAngles(0.0f, angleFromCenter));
                speedBoost = Mathf.Clamp(speedBoost - (angleFromCenter * 2.0f), 0.0f, 1.2f);

                // Debug.Log("Speed boost: " + speedBoost + ", angle: " + angleFromCenter);

                speed = FireSpeed * speedBoost;
            }

            // set the current arrow to be a normal physics object (not kinematic) and set it's velocity
            currentArrow.GetComponent<Rigidbody2D>().isKinematic = false;
            currentArrow.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            currentArrow.GetComponent<Rigidbody2D>().velocity =
                currentArrow.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().velocity = BowShaft.transform.rotation * new Vector2(-speed, 0.0f);

            // reset bow state to idle
            drawingBow = false;
            RenderBowString(Vector3.zero);
        }

        private void Start()
        {
            // assign component references for fast use later
            bowStringLineRenderer1 = BowString.transform.GetChild(0).GetComponent<LineRenderer>();
            bowStringLineRenderer2 = BowString.transform.GetChild(1).GetComponent<LineRenderer>();
            audioSource = GetComponent<AudioSource>();
            RenderBowString(Vector3.zero);
            startAngle = BowShaft.transform.eulerAngles.z;
        }

        private void Update()
        {
            // lower the cooldown, when this goes 0 or less, the bow can fire again
            cooldownTimer -= Time.deltaTime;

            // check for mouse down
            if (Input.GetMouseButtonDown(0))
            {
                if (cooldownTimer <= 0.0f)
                {
                    Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (DrawStartArea.OverlapPoint(worldPos))
                    {
                        BeginBowDraw();
                    }
                }
            }
            // check for mouse staying down
            else if (Input.GetMouseButton(0))
            {
                if (drawingBow)
                {
                    ContinueBowDraw();
                }
            }

            // check for mouse up - this is not another else if because the mouse could in theory go up in the same frame it went down
            if (Input.GetMouseButtonUp(0))
            {
                if (drawingBow)
                {
                    FireArrow(false);
                }
            }
        }
    }
}