using UnityEngine;

public class MineDetecter : MonoBehaviour
{
    [SerializeField] private GameObject decalDetecterGreen;
    [SerializeField] private GameObject decalDetecterRed;
    [SerializeField] private float speed = 1.5f;

    [SerializeField] private float rangeToShoot = 1f;   
    private float cooldownTime = 1f;    
    private float lastShootTime = -1f;  

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        if (Time.time - lastShootTime >= cooldownTime)
        {
            DropDecal();
            lastShootTime = Time.time; 
        }
    }

    private void DropDecal()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rangeToShoot))
        {
            int hitLayer = hitInfo.collider.gameObject.layer;

            if (hitLayer == LayerMask.NameToLayer("Floor"))
            {
                Quaternion decalRotation = Quaternion.LookRotation(hitInfo.normal);

                Instantiate(decalDetecterGreen, hitInfo.point, decalRotation);
            }
            
            else if (hitLayer == LayerMask.NameToLayer("Mine"))
            {
                Instantiate(decalDetecterRed, hitInfo.point, Quaternion.identity);
            }
        }
    }
}
