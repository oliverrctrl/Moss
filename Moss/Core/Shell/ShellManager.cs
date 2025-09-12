using System;
using System.Collections.Generic;
using Moss.User.Shell;

namespace Moss.User.Shell;

/// <summary>
/// This contains one or more shell instances, this also helps to differentiate the topbar from each shell instance.
/// </summary>
public class ShellManager
{
    public static ShellManager Instance { get; private set; }
    
    public List<ShellInstance> Shells = new();
    public ShellInstance TargetInstance;

    public ShellManager()
    {
        Instance = this;
    }
    public void Start()
    {
        
        Shells.Add(new ShellInstance());
    }

    public void Update()
    {
        foreach (var i in Shells)
        {
            i.Update();
        }
    }

    public ShellInstance SpawnShell(bool target = true)
    {
        var shell = new ShellInstance();
        Shells.Add(shell);
        if (target)
            TargetInstance = shell;
        return shell;
    }
}