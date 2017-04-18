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

            {
                float leftStickX = Input.GetAxis("Joystick Axis, X Axis");
                float leftStickY = Input.GetAxis("Joystick Axis, Y Axis");
                LeftStick = new Vector2(leftStickX, leftStickY);
            }

            {
                float rightStickX = Input.GetAxis("Joystick Axis, 4th Axis");
                float rightStickY = Input.GetAxis("Joystick Axis, 5th Axis");
                RightStick = new Vector2(rightStickX, rightStickY);
            }

            AButton = Input.GetKey("joystick button 0");
        }

        public override void Reset()
        {
            base.Reset();

            LeftStick = Vector2.zero;
            RightStick = Vector2.zero;
            AButton = false;
        }
    }
}