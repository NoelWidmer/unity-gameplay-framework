using UnityEngine;

namespace GameplayFramework.Sample
{
    public class PlayerInputSample : PlayerInput
    {
        public Vector2 LeftStick
        {
            get; protected set;
        }

        public Vector2 RightStick
        {
            get; protected set;
        }

        public bool AButton
        {
            get; protected set;
        }



        protected override void Tick(TickArgs e)
        {
            base.Tick(e);

            // Left Stick
            {
                float leftStickX = Input.GetAxis("LeftAxisX");
                float leftStickY = Input.GetAxis("LeftAxisY");
                LeftStick = new Vector2(leftStickX, leftStickY);
            }

            // Right Stick
            {
                float rightStickX = Input.GetAxis("RightAxisX");
                float rightStickY = Input.GetAxis("RightAxisY");
                RightStick = new Vector2(rightStickX, rightStickY);
            }

            AButton = Input.GetButton("AButton");
        }
    }
}