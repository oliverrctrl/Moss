using System;
using System.Linq;
using Moss.core.interfaces;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Moss.core.attributes;
using Moss.user.shell;

namespace Moss.user.programs;

[Program("fs", "Filesystem Utility", "Program used to interact with the virtual file system")]
public class Filesystem : ISharpProgram
{ public void Run(string[] args)
    {
        if (Kernel.Vfs == null) return;
        
        var disks = VFSManager.GetDisks();
        
        if (args[0] == null)
            throw new ArgumentNullException(args[0]);
        
        switch (args[0])
        {
            case "list":
            {
                ShellManager.Instance.TargetInstance.WriteLine("== Disks ==");
                for (var i = 0; i < disks.Count; i++)
                {
                    var disk = disks[i];
                    ShellManager.Instance.TargetInstance.WriteLine($"Disk {i} =============");
                    ShellManager.Instance.TargetInstance.WriteLine($"Size: {disk.Size}");
                    ShellManager.Instance.TargetInstance.WriteLine($"Partitions: {disk.Partitions.Count}");
                    for (var x = 0; x < disk.Partitions.Count; x++)
                    {
                        var partition = disk.Partitions[x];
                        ShellManager.Instance.TargetInstance.WriteLine($"\tPartition {x}");
                        ShellManager.Instance.TargetInstance.WriteLine($"\tPath?: {partition.RootPath}");
                        ShellManager.Instance.TargetInstance.WriteLine($"\tHasFS?: {partition.HasFileSystem}");
                    }
                }

                break;
            }
            case "disk" when args[1] == null:
                throw new ArgumentNullException(args[1]);
            case "disk":
            {
                var diskNum = int.Parse(args[1]);
                if (diskNum ! <= VFSManager.GetDisks().Count)
                {
                    if (args[2] == null) throw new ArgumentNullException(args[2]);
                    switch (args[2])
                    {
                        case "format":
                            ShellManager.Instance.TargetInstance.ForegroundColor = ConsoleColor.Yellow;
                            ShellManager.Instance.TargetInstance.WriteLine($"WARNING! ALL data will be wiped on drive {diskNum}!");
                            ShellManager.Instance.TargetInstance.WriteLine("Would you like to proceed? Y/N");
                            ShellManager.Instance.TargetInstance.ForegroundColor = Kernel.ShellManager.TargetInstance.ForegroundColor;
                            switch (ShellManager.Instance.TargetInstance.ReadKey().Key)
                            {
                                case ConsoleKey.Y:
                                    disks[diskNum].Clear();
                                    ShellManager.Instance.TargetInstance.WriteLine("Cleared partitions");
                                    disks[diskNum].CreatePartition((int)disks[diskNum].Size / (1024 * 1024));
                                    ShellManager.Instance.TargetInstance.WriteLine("Created single partition");
                                    var quick = args.Length > 4 && args[4] == "--quick";
                                    disks[diskNum].FormatPartition(diskNum, args[3], quick: quick);
                                    ShellManager.Instance.TargetInstance.WriteLine("Formatted whole drive successfully");
                                    break;
                            }

                            break;
                    }
                }
                else
                    throw new IndexOutOfRangeException(args[1]);

                break;
            }
            default:
                throw new ArgumentException("Invalid argument");
        }
    }

    public void Help()
    {
        ShellManager.Instance.TargetInstance.WriteLine("\tlist - List all known disks");
        ShellManager.Instance.TargetInstance.WriteLine("\tdisk (number)" +
                          "\n \t format (format) (--quick) - Formats a disk & creates a single partition");
    }
}