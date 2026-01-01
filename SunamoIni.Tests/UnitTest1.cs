namespace SunamoIni.Tests;

/// <summary>
/// EN: Unit tests for IniFile class
/// CZ: Unit testy pro třídu IniFile
/// </summary>
public class UnitTest1
{
    /// <summary>
    /// EN: Test writing and reading values from INI file
    /// CZ: Test zápisu a čtení hodnot z INI souboru
    /// </summary>
    [Fact]
    public void WriteIni()
    {
        string testFilePath = @"E:\vs\Projects\PlatformIndependentNuGetPackages.Tests\SunamoIni.Tests\test.ini";
        IniFile iniFile = new IniFile(testFilePath);

        string expectedSection = "Section";
        string expectedKey = "Key";
        string expectedValue = "Value";

        iniFile.IniWriteValue(expectedSection, expectedKey, expectedValue);

        string actualValue = iniFile.IniReadValue(expectedSection, expectedKey);

        Assert.Equal(expectedValue, actualValue);
    }
}
