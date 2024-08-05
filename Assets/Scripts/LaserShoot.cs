using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    [SerializeField] private float laserSpeed;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && PauseMenu.GameIsPaused == false)
        {
            SoundManager.PlaySound(SoundType.LASER, 0.1f);
            LaserPewPew();
        }
    }
    void LaserPewPew()
    {
        
        Rigidbody laserRB = Instantiate(laser, transform.position, transform.rotation).GetComponent<Rigidbody>();
        laserRB.AddForce(transform.forward * laserSpeed, ForceMode.Impulse);
        Destroy(laserRB.gameObject, 5f);
    }
}
