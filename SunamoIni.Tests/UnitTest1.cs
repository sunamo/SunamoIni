// variables names: ok
namespace SunamoIni.Tests;

/// <summary>
/// Unit tests for IniFile class
/// </summary>
public class UnitTest1
{
    /// <summary>
    /// Test writing and reading values from INI file
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
