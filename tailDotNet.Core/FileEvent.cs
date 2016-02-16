namespace tailDotNet
{
	public enum FileEvent
	{
		/// <summary>
		/// The tailed file has be renamed
		/// </summary>
		Renamed = 1,
		/// <summary>
		/// A previously tailed file could not be found
		/// </summary>
		NotFound = 2,
		/// <summary>
		/// The tailed file has been deleted
		/// </summary>
		Deleted = 3,
		/// <summary>
		/// The tailed file has new (trailing) content
		/// </summary>
		TailGrown = 4
	}
}