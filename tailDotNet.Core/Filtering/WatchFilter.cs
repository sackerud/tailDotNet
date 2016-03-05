namespace tailDotNet.Filtering
{
	public class WatchFilter
	{
		public Filter InclusionFilter { get; set; }
		public Filter ExclusionFilter { get; set; }
	}

	public class Filter
	{
		public string SimpleFilter { get; set; }
		public string RegExFilter { get; set; }
	}
}