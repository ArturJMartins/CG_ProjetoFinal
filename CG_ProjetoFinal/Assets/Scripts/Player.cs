using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float  maxForwardSpeed;
    [SerializeField] private float  maxBackwardSpeed;
    [SerializeField] private float  maxStrafeSpeed;
    [SerializeField] private float  maxLookUpAngle;
    [SerializeField] private float  maxLookDownAngle;
    private CharacterController     controller;
    private Transform               head;
    private Vector3                 headRotation;
    private Vector3                 velocity;
    private Vector3                 motion;

    [Header("Decal")]
    [SerializeField] private GameObject decalGameObject;
    [SerializeField] private float      rangeToShoot = 5f;
    [Header("MineDetecter")]
    [SerializeField] private GameObject mineDetecter;
    private Vector3 mineDetecterPosition;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        head       = GetComponentInChildren<Camera>().transform;
        mineDetecterPosition = new Vector3 (0, 0.55f,2);
    }

    private void Start()
    {
        HideCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = 1f;
        }
        if (Time.timeScale == 0)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            ShootDecal();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMineSweeper();
        }

        UpdateRotation();
        UpdateHead();
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
        UpdatePosition();
    }

    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X");

        transform.Rotate(0f, rotation, 0f);
    }

    private void UpdateHead()
    {
        headRotation = head.localEulerAngles;
        
        headRotation.x -= Input.GetAxis("Mouse Y");

        if (headRotation.x > 180f)
            headRotation.x = Mathf.Max(maxLookUpAngle, headRotation.x);
        else
            headRotation.x = Mathf.Min(maxLookDownAngle, headRotation.x);

        head.localEulerAngles = headRotation;
    }

    private void UpdateVelocity()
    {
        float forwardAxis   = Input.GetAxis("Forward");
        float strafeAxis    = Input.GetAxis("Strafe");

        if (forwardAxis >= 0f)
            velocity.z = forwardAxis * maxForwardSpeed;
        else
            velocity.z = forwardAxis * maxBackwardSpeed;

        velocity.x = strafeAxis * maxStrafeSpeed;

        if (velocity.magnitude > maxForwardSpeed)
            velocity = velocity.normalized * (forwardAxis > 0 ? 
                maxForwardSpeed : maxBackwardSpeed);
    }

    private void UpdatePosition()
    {
        motion = transform.TransformVector(velocity * Time.fixedDeltaTime);

        controller.Move(motion);
    }

    private void ShootDecal()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rangeToShoot))
        {
            Quaternion decalRotation = Quaternion.LookRotation(hitInfo.normal);

            GameObject decal = Instantiate(decalGameObject, hitInfo.point, decalRotation);
        }
    }

    private void SendMineSweeper()
    {
        Instantiate(mineDetecter,mineDetecterPosition,quaternion.identity);
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
}
