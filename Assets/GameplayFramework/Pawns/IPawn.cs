using System;

namespace GameplayFramework
{
    /// <summary>
    ///     Interface for working with pawns.
    /// </summary>
    public interface IPawn
    {
        /// <summary>
        ///     The controller that possesses this pawn or null if this pawn is unpossessed.
        /// </summary>
        Controller Controller
        {
            get;
        }

        /// <summary>
        ///     Called when this pawn became possessed by a controller.
        /// </summary>
        event EventHandler BecamePossessed;

        /// <summary>
        ///     Called when this pawn became unpossessed by a controller.
        /// </summary>
        event EventHandler BecameUnPossessed;

        /// <summary>
        ///     Called by the controller when this pawn became possessed by it.
        /// </summary>
        /// <param name="controller">The controller that took possession of this pawn.</param>
        void OnBecamePossessed(Controller controller);
    }
}