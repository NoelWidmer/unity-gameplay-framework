namespace GameplayFramework
{
    /// <summary>
    ///     Interface for working with pawns.
    /// </summary>
    public interface IPawn
    {
        /// <summary>
        ///     Called when this pawn becomes possessed.
        /// </summary>
        /// <param name="controller">The controller that took possession of this pawn.</param>
        void OnBecamePossessed(Controller controller);
    }
}