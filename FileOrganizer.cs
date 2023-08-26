namespace FileOrganizerConsole
{
  class FileOrganizer
  {
    public string ConditionType { get; set; }
    public string[] ValueToCompare { get; set; }
    public int[] CharacterPosition { get; set; }
    public string ComparisonOperator { get; set; }
    public string SourceFolder { get; set; }
    public string DestinationFolder { get; set; }

    public FileOrganizer(string ConditionType, string[] ValueToCompare, int[] CharacterPosition, string ComparisonOperator, string SourceFolder, string DestinationFolder)
    {
      this.ConditionType = ConditionType;
      this.ValueToCompare = ValueToCompare;
      this.CharacterPosition = CharacterPosition;
      this.ComparisonOperator = ComparisonOperator;
      this.SourceFolder = SourceFolder;
      this.DestinationFolder = DestinationFolder;
    }
  }
}