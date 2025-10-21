// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoIni;

/// <summary>
///     Create a New INI file to store or load data
/// </summary>
public class IniFile
{
    // TODO: Balíček byl nekompatibilní s netstd, nainstalovat nový a odkomentovat
    private readonly Configuration conf;
    public string path;
    public IniFile(string INIPath)
    {
        path = INIPath;
        //ip = new IniParser(path);
        try
        {
            conf = Configuration.LoadFromFile(path);
        }
        catch (Exception ex)
        {
        }
    }
    //IniParser ip = null;
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section,
        string key, string def, StringBuilder retVal,
        int size, string filePath);
    /// <summary>
    ///     Zápis probíhá jen pomocí W32 metod, tam pokud vím se s tím ještě nevyskytly problémy
    /// </summary>
    /// <param name="Section"></param>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    public void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, path);
    }
    /// <summary>
    ///     Vždy používá sharp config, předtím vždy používala W32 metody
    /// </summary>
    /// <param name="Section"></param>
    /// <param name="Key"></param>
    public string IniReadValue(string Section, string Key)
    {
        return IniReadValue(true, Section, Key);
    }
    public string IniReadValueSharpConfig(string Section, string Key)
    {
        return IniReadValue(true, Section, Key);
    }
    /// <summary>
    ///     A1 zda se má použít SharpConfig
    /// </summary>
    /// <param name="useIniParser"></param>
    /// <param name="Section"></param>
    /// <param name="Key"></param>
    public string IniReadValue(bool useIniParser, string Section, string Key)
    {
        if (useIniParser)
        {
            // TODO: Balíček byl nekompatibilní s netstd, nainstalovat nový a odkomentovat
            if (conf != null) return conf[Section][Key].StringValue;
            return "";
        }
        var temp = new StringBuilder(255);
        var i = GetPrivateProfileString(Section, Key, "", temp,
            int.MaxValue, path);
        return temp.ToString();
    }
    public static IniFile InStartupPath(string iniFilePath)
    {
        // TODO: Balíček byl nekompatibilní s netstd, nainstalovat nový a odkomentovat
        return new IniFile(iniFilePath);
    }
}