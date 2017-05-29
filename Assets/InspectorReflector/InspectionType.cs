namespace InspectorReflector
{
    /// <summary>
    ///     This type supports various ways to customize IR visuals.
    /// </summary>
    public enum InspectionType
    {
        /// <summary>
        ///     Default inspection.
        /// </summary>
        Default,
        /// <summary>
        ///     The IR won't allow writing to the member.
        /// </summary>
        Readonly,
        /// <summary>
        ///     The IR won't allow writing to the member. The text displayed can be selected and copyied.
        /// </summary>
        ReadonlySelectable,
        /// <summary>
        ///     An asset from the project can be droped to this IR field. Scene objects are not allowed.
        /// </summary>
        DropableObject,
        /// <summary>
        ///     An asset from the project can be droped to this IR field. Scene objects are allowed.
        /// </summary>
        DropableObjectAllowSceneObjects,
        /// <summary>
        ///     An asset from the project can be droped to this IR field. Scene objects are allowed.
        /// </summary>
        DelayedDouble,
        DelayedFloat,
        DelayedInt,
        DelayedString,
        TagString,
        AreaString
    }
}