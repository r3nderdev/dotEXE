using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserShoot : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    [SerializeField] private float laserSpeed = 60;
    [SerializeField] private float cooldown = 0.05f;

    [SerializeField] private InputAction shootAction;

    private bool canShoot = true;

    private void OnEnable()
    {
        shootAction.Enable();
        shootAction.performed += OnShoot;
    }

    private void OnDisable()
    {
        shootAction.Disable();
        shootAction.performed -= OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (canShoot && !PauseMenu.GameIsPaused)
        {
            canShoot = false;
            StartCoroutine(ShootLaser());
        }
    }

    private IEnumerator ShootLaser()
    {
        SoundManager.PlaySound(SoundType.LASER, 0.1f);

        Rigidbody laserRB = Instantiate(laser, transform.position, transform.rotation).GetComponent<Rigidbody>();
        laserRB.AddForce(transform.forward * laserSpeed, ForceMode.Impulse);

        Destroy(laserRB.gameObject, 5f);

        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}