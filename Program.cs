namespace FileOrganizerConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      Program program = new Program();
      FileOrganizer fileOrganizer = program.CreateFileOrganizer();

      program.ExecuteFileOrganizer(fileOrganizer);
    }

    FileOrganizer CreateFileOrganizer()
    {
      string[] ConditionTypes = { "starts with", "ends with", "character position" };
      string[] ComparisonOperators = { "equals to", "does not equal to", "less than", "less than or equal", "more than", "more than or equal", "between" };

      int InputConditionIndex = GetInputConditionIndex(ConditionTypes);
      string[] InputValueToCompare = new string[2];
      int[] InputCharacterPosition = new int[2];
      int InputComparisonOperatorIndex = 0;
      string InputCondition = "";
      string InputComparisonOperator = "";
      string InputSourceFolder = "";
      string InputDestinationFolder = "";

      bool ConfirmCreateFileOrganizer = false;

      while (!ConfirmCreateFileOrganizer)
      {
        if (InputConditionIndex == 0 || InputConditionIndex == 1)
        {
          InputValueToCompare = GetInputValueToCompare(1);
        }
        else if (InputConditionIndex == 2)
        {
          InputCharacterPosition = GetInputCharacterPosition();
          InputComparisonOperatorIndex = GetInputComparisonOperatorIndex(ComparisonOperators);

          if (InputComparisonOperatorIndex >= 0 && InputComparisonOperatorIndex <= 5)
          {
            InputValueToCompare = GetInputValueToCompare(1);
          }
          else if (InputComparisonOperatorIndex == 6)
          {
            InputValueToCompare = GetInputValueToCompare(2);
          }
        }

        InputCondition = ConditionTypes[InputConditionIndex];
        InputComparisonOperator = ComparisonOperators[InputComparisonOperatorIndex];

        Console.Write("Enter source folder: ");
        InputSourceFolder = Console.ReadLine();
        Console.WriteLine();

        Console.Write("Enter destination folder: ");
        InputDestinationFolder = Console.ReadLine();
        Console.WriteLine();

        bool IsInputClean = ValidateInput(ConditionTypes, InputConditionIndex, InputValueToCompare, InputCharacterPosition, InputComparisonOperatorIndex, ComparisonOperators, InputSourceFolder, InputDestinationFolder);
        if (IsInputClean)
        {
          PrintTaskPreview(InputCondition, InputConditionIndex, InputComparisonOperator, InputComparisonOperatorIndex, InputValueToCompare, InputCharacterPosition, InputSourceFolder, InputDestinationFolder);
          Console.WriteLine($"[Press 1] = Yes");
          Console.WriteLine($"[Press 0] = No");
          Console.Write("Proceed? ");
          string InputConfirmCreateFileOrganizer = Console.ReadLine();
          Console.WriteLine();

          if (InputConfirmCreateFileOrganizer.Equals("1"))
          {
            ConfirmCreateFileOrganizer = true;
            InputCharacterPosition = FixCharacterPositionForSubstring(InputCharacterPosition);
          }
        }
      }

      return new FileOrganizer(InputCondition, InputValueToCompare, InputCharacterPosition, InputComparisonOperator, InputSourceFolder, InputDestinationFolder);
    }

    int GetInputConditionIndex(string[] ConditionTypes)
    {
      for (int index = 0; index < ConditionTypes.Length; index++)
      {
        Console.WriteLine($"[Enter {index}] - {ConditionTypes[index]}");
      }
      Console.Write("Choose how you want to validate the filename: ");
      int InputConditionIndex = Int32.Parse(Console.ReadLine());
      Console.WriteLine();

      return InputConditionIndex;
    }

    string[] GetInputValueToCompare(int ValueToCompareSize)
    {
      string[] InputValueToCompare = new string[ValueToCompareSize];
      for (int index = 0; index < ValueToCompareSize; index++)
      {
        Console.Write($"[Value {index}] - Enter the value to compare: ");
        InputValueToCompare[index] = Console.ReadLine();
      }
      Console.WriteLine();

      return InputValueToCompare;
    }

    int[] GetInputCharacterPosition()
    {
      int[] InputCharacterPosition = new int[2];
      Console.Write("Enter starting character position: ");
      InputCharacterPosition[0] = Int32.Parse(Console.ReadLine());
      Console.WriteLine();

      Console.Write("Enter ending character position: ");
      InputCharacterPosition[1] = Int32.Parse(Console.ReadLine());
      Console.WriteLine();

      return InputCharacterPosition;
    }

    int GetInputComparisonOperatorIndex(string[] ComparisonOperators)
    {
      for (int index = 0; index < ComparisonOperators.Length; index++)
      {
        Console.WriteLine($"[Enter {index}] - {ComparisonOperators[index]}");
      }
      Console.Write("Choose how do you want to compare the value: ");
      int InputComparisonOperatorIndex = Int32.Parse(Console.ReadLine());
      Console.WriteLine();

      return InputComparisonOperatorIndex;
    }

    bool ValidateInput(string[] ConditionTypes, int InputConditionIndex, string[] InputValueToCompare, int[] InputCharacterPosition, int InputComparisonOperatorIndex, string[] ComparisonOperators, string InputSourceFolder, string InputDestinationFolder)
    {
      if (InputConditionIndex < 0 || InputConditionIndex >= ConditionTypes.Length)
      {
        Console.WriteLine("Sorry, the selected condition type is not available.");
        return false;
      }

      if (InputConditionIndex == 0 || InputConditionIndex == 1)
      {
        if (InputValueToCompare[0].Trim().Length == 0)
        {
          Console.WriteLine("Value to compare cannot be left empty.");
          return false;
        }
      }
      else if (InputConditionIndex == 2)
      {
        if (InputCharacterPosition[0] < 0 || InputCharacterPosition[1] < 0)
        {
          Console.WriteLine("Character position cannot be negative.");
          return false;
        }

        if (InputCharacterPosition[1] - InputCharacterPosition[0] < 0)
        {
          Console.WriteLine("Ending character position must not be smaller than the starting character position.");
          return false;
        }

        if (InputComparisonOperatorIndex < 0 || InputComparisonOperatorIndex >= ComparisonOperators.Length)
        {
          Console.WriteLine("Sorry, the selected comparison operator is not available.");
          return false;
        }

        if (InputComparisonOperatorIndex >= 2 && InputComparisonOperatorIndex <= 5)
        {
          if (InputValueToCompare[0].Trim().Length == 0)
          {
            Console.WriteLine("Value to compare cannot be left empty.");
            return false;
          }

          if (!Int32.TryParse(InputValueToCompare[0], out _))
          {
            Console.WriteLine("Sorry, the input value must be a number.");
            return false;
          }
        }
        else if (InputComparisonOperatorIndex == 6)
        {
          if ((InputValueToCompare[0].Trim().Length == 0) || (InputValueToCompare[1].Trim().Length == 0))
          {
            Console.WriteLine("Value to compare cannot be left empty.");
            return false;
          }

          if (!Int32.TryParse(InputValueToCompare[0], out _) || !Int32.TryParse(InputValueToCompare[1], out _))
          {
            Console.WriteLine("Sorry, the input value must be a number.");
            return false;
          }
        }
      }

      if (InputSourceFolder.Trim().Length == 0)
      {
        Console.WriteLine("Source folder cannot be left empty.");
        return false;
      }

      if (InputDestinationFolder.Trim().Length == 0)
      {
        Console.WriteLine("Destination folder cannot be left empty.");
        return false;
      }

      return true;
    }

    void PrintTaskPreview(string InputCondition, int InputConditionIndex, string InputComparisonOperator, int InputComparisonOperatorIndex, string[] InputValueToCompare, int[] InputCharacterPosition, string InputSourceFolder, string InputDestinationFolder)
    {
      if (InputConditionIndex == 0 || InputConditionIndex == 1)
      {
        Console.WriteLine($"Filename that {InputCondition} {InputValueToCompare[0]}");
      }
      else if (InputConditionIndex == 2)
      {
        if (InputComparisonOperatorIndex >= 0 && InputComparisonOperatorIndex <= 5)
        {
          Console.WriteLine($"Filename that has {InputCondition} from {InputCharacterPosition[0]} to {InputCharacterPosition[1]} where the value {InputComparisonOperator} {InputValueToCompare[0]}");
        }
        else if (InputComparisonOperatorIndex == 6)
        {
          Console.WriteLine($"Filename that has {InputCondition} from {InputCharacterPosition[0]} to {InputCharacterPosition[1]} where the value {InputComparisonOperator} {InputValueToCompare[0]} and {InputValueToCompare[1]}");
        }
      }
      Console.WriteLine();
    }

    int[] FixCharacterPositionForSubstring(int[] InputCharacterPosition)
    {
      // Reduce the input by 1 to match the string index
      // The length of substring can be retrieved by substracting the starting position with the ending position
      // The second item inside InputCharacterPosition is to store the length of substring
      InputCharacterPosition[0] = InputCharacterPosition[0] - 1;
      InputCharacterPosition[1] = InputCharacterPosition[1] - InputCharacterPosition[0];
      return InputCharacterPosition;
    }

    void ExecuteFileOrganizer(FileOrganizer fileOrganizer)
    {
      DirectoryInfo InputDirectory = new DirectoryInfo(fileOrganizer.SourceFolder);
      DirectoryInfo OutputDirectory = new DirectoryInfo(fileOrganizer.DestinationFolder);

      if (!InputDirectory.Exists)
      {
        return;
      }

      FileInfo[] FilesToOrganize = GetFilesToOrganize(fileOrganizer, InputDirectory);

      CreateDirectory(OutputDirectory);
      CategorizeFiles(FilesToOrganize, OutputDirectory);
      Console.WriteLine($"Operation completed! All {FilesToOrganize.Length} are copied into {OutputDirectory}");
    }

    FileInfo[] GetFilesToOrganize(FileOrganizer fileOrganizer, DirectoryInfo InputDirectory)
    {
      FileInfo[] InputFiles = InputDirectory.GetFiles();
      FileInfo[] FilesToOrganize = null;

      if (fileOrganizer.ConditionType.Equals("starts with"))
      {
        FilesToOrganize = Array.FindAll(InputFiles, InputFile => InputFile.Name.StartsWith(fileOrganizer.ValueToCompare[0], false, null));
      }
      else if (fileOrganizer.ConditionType.Equals("ends with"))
      {
        FilesToOrganize = Array.FindAll(InputFiles, InputFile => InputFile.Name.EndsWith(fileOrganizer.ValueToCompare[0], false, null));
      }
      else if (fileOrganizer.ConditionType.Equals("character position"))
      {
        if (fileOrganizer.ComparisonOperator.Equals("equals to"))
        {
          FilesToOrganize = Array.FindAll(InputFiles, InputFile => InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1]).Equals(fileOrganizer.ValueToCompare[0]));
        }
        else if (fileOrganizer.ComparisonOperator.Equals("does not equal to"))
        {
          FilesToOrganize = Array.FindAll(InputFiles, InputFile => !InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1]).Equals(fileOrganizer.ValueToCompare[0]));
        }
        else if (fileOrganizer.ComparisonOperator.Equals("less than"))
        {
          FilesToOrganize = Array.FindAll(InputFiles, InputFile => Int32.Parse(InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1])) < Int32.Parse(fileOrganizer.ValueToCompare[0]));
        }
        else if (fileOrganizer.ComparisonOperator.Equals("less than or equal"))
        {
          FilesToOrganize = Array.FindAll(InputFiles, InputFile => Int32.Parse(InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1])) <= Int32.Parse(fileOrganizer.ValueToCompare[0]));
        }
        else if (fileOrganizer.ComparisonOperator.Equals("more than"))
        {
          FilesToOrganize = Array.FindAll(InputFiles, InputFile => Int32.Parse(InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1])) > Int32.Parse(fileOrganizer.ValueToCompare[0]));
        }
        else if (fileOrganizer.ComparisonOperator.Equals("more than or equal"))
        {
          FilesToOrganize = Array.FindAll(InputFiles, InputFile => Int32.Parse(InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1])) >= Int32.Parse(fileOrganizer.ValueToCompare[0]));
        }
        else if (fileOrganizer.ComparisonOperator.Equals("between"))
        {
          FilesToOrganize = Array.FindAll(InputFiles, InputFile => Int32.Parse(InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1])) >= Int32.Parse(fileOrganizer.ValueToCompare[0]) && Int32.Parse(InputFile.Name.Substring(fileOrganizer.CharacterPosition[0], fileOrganizer.CharacterPosition[1])) <= Int32.Parse(fileOrganizer.ValueToCompare[1]));
        }
      }

      return FilesToOrganize;
    }

    void CreateDirectory(DirectoryInfo OutputDirectory)
    {
      if (OutputDirectory.Exists) { OutputDirectory.Delete(true); }
      OutputDirectory.Create();
    }

    void CategorizeFiles(FileInfo[] FilesToOrganize, DirectoryInfo OutputDirectory)
    {
      foreach (FileInfo FileToOrganize in FilesToOrganize)
      {
        string destinationPath = OutputDirectory + "\\";
        FileToOrganize.CopyTo(destinationPath + FileToOrganize.Name);
      }
    }
  }
}