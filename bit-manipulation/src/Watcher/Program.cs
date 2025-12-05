using System.Diagnostics;
using System.Reflection;

var baseDirectory = AppContext.BaseDirectory;
var testAssemblies = new HashSet<string>();

foreach (var dll in Directory.GetFiles(baseDirectory, "*.dll"))
{
    try 
    {   
        var assembly = Assembly.LoadFrom(dll);

        var isTest =
            assembly.FullName is not null
            && assembly.GetReferencedAssemblies()
                .Any(a => (a.Name?.Contains("xunit", StringComparison.InvariantCultureIgnoreCase) ?? false)
                          || (a.Name?.Contains("nunit", StringComparison.InvariantCultureIgnoreCase) ?? false)
                          || (a.Name?.Contains("MSTest", StringComparison.InvariantCultureIgnoreCase) ?? false));

        if (isTest)
        {
            testAssemblies.Add(dll);
        }

    } catch 
    {
        // ignored
    }
}

var startInfo = new ProcessStartInfo(fileName: "dotnet", arguments: ["test", ..testAssemblies]);
Process.Start(startInfo)?.WaitForExit();
