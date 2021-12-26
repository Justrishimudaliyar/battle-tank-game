﻿using UnityEngine;

public class PlayerTankController 
{
    public PlayerTankModel TankModel { get; }
    public PlayerTankView TankView { get; }

    private Rigidbody tankRigidbody;
    private Joystick rightJoystick;
    private Joystick leftJoystick;
    private Camera camera;

    public PlayerTankController(PlayerTankModel tankModel, PlayerTankView tankPrefab)
    {
        TankModel = tankModel;
        TankView = GameObject.Instantiate<PlayerTankView>(tankPrefab);
        tankRigidbody = TankView.GetComponent<Rigidbody>();
        TankView.SetTankControllerReference(this);
    }

    public void SetJoystickReference(Joystick rightRef, Joystick leftRef)
    {
        rightJoystick = rightRef;
        leftJoystick = leftRef;
    }

    public void SetCameraReference(Camera cameraRef)
    {
        camera = cameraRef;
        camera.transform.SetParent(TankView.Turret.transform);
    }

    public void FixedUpdateTankController()
    {
        if(tankRigidbody)
        {
            if(leftJoystick.Vertical != 0)
            {
                AddForwardMovementInput();
            }
            if(leftJoystick.Horizontal != 0)
            {
                AddRotationInput();
            }
        }

        if(TankView.Turret)
        {
            if(rightJoystick.Horizontal != 0)
            {
                AddTurretRotationInput();
            }
        }

    }

    private void AddForwardMovementInput()
    {
        Vector3 forwardInput = tankRigidbody.transform.position + leftJoystick.Vertical * tankRigidbody.transform.forward * TankModel.Speed * Time.deltaTime;

        tankRigidbody.MovePosition(forwardInput);
    }

    private void AddRotationInput()
    {
        Quaternion desiredRotation = tankRigidbody.transform.rotation * Quaternion.Euler(Vector3.up * leftJoystick.Horizontal * TankModel.RotationRate * Time.deltaTime);

        tankRigidbody.MoveRotation(desiredRotation);
    }

    private void AddTurretRotationInput()
    {
        Vector3 desiredRotation = Vector3.up * rightJoystick.Horizontal * TankModel.TurretRotationRate * Time.deltaTime;

        TankView.Turret.transform.Rotate(desiredRotation, Space.Self);
    }

    public void TakeDamage(int damage)
    {      
        TankModel.Health -= damage;
        SetHealthUI();

        if (TankModel.Health - damage <= 0 && !TankModel.b_IsDead)
        {
            Death();
        }
    }

    public void SetHealthUI()
    {
        TankView.HealthSlider.value = TankModel.Health;

        TankView.FillImage.color = Color.Lerp(TankModel.ZeroHealthColor, TankModel.FullHealthColor, TankModel.Health / TankModel.MaxHealth);
    }

    private void Death()
    {
        TankModel.b_IsDead = true;

        TankView.explosionParticles.transform.position = TankView.transform.position;
        TankView.explosionParticles.gameObject.SetActive(true);
        TankView.explosionParticles.Play();
        TankView.explosionSound.Play();

        TankView.Death();
    }

}
