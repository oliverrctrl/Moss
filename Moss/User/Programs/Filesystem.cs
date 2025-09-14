using System;
using System.Linq;
using Moss.Core.Interfaces;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Moss.Core.Attributes;
using Moss.Core.Graphics;
using Moss.Core.Shell;

namespace Moss.User.Programs;

[Program("fs", "Filesystem Utility", "Program used to interact with the virtual file system")]
public class Filesystem : ISharpProgram
{
    public Window Window { get; set; }
    public WindowCanvas Canvas { get; set; }

    public void Run(string[] args)
    {
        if (Kernel.Vfs == null) return;
        
        var disks = VFSManager.GetDisks();
        
        if (args[0] == null)
            throw new ArgumentNullException(args[0]);
        
        switch (args[0])
        {
            case "list":
            {
                Canvas.Write("== Disks ==");
                for (var i = 0; i < disks.Count; i++)
                {
                    var disk = disks[i];
                    Canvas.Write($"Disk {i} =============");
                    Canvas.Write($"Size: {disk.Size}");
                    Canvas.Write($"Partitions: {disk.Partitions.Count}");
                    for (var x = 0; x < disk.Partitions.Count; x++)
                    {
                        var partition = disk.Partitions[x];
                        Canvas.Write($"\tPartition {x}");
                        Canvas.Write($"\tPath?: {partition.RootPath}");
                        Canvas.Write($"\tHasFS?: {partition.HasFileSystem}");
                    }
                }

                break;
            }
            case "disk":
            {
                var diskNum = int.Parse(args[1]);
                if (diskNum ! <= VFSManager.GetDisks().Count)
                {
                    if (args[2] == null) throw new ArgumentNullException(args[2]);
                    switch (args[2])
                    {
                        case "format":
                            Canvas.Write($"WARNING! ALL data will be wiped on drive {diskNum}!");
                            Canvas.Write("Would you like to proceed? Y/N");
                            
                            /*
                            switch (Canvas.ReadKey().Key)
                            {
                                case ConsoleKey.Y:
                                    disks[diskNum].Clear();
                                    Canvas.Write("Cleared partitions");
                                    disks[diskNum].CreatePartition((int)disks[diskNum].Size / (1024 * 1024));
                                    Canvas.Write("Created single partition");
                                    var quick = args.Length > 4 && args[4] == "--quick";
                                    disks[diskNum].FormatPartition(diskNum, args[3], quick: quick);
                                    Canvas.Write("Formatted whole drive successfully");
                                    break;
                            }
                            */

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
        Canvas.Write("\tlist - List all known disks");
        Canvas.Write("\tdisk (number)" +
                          "\n \t format (format) (--quick) - Formats a disk & creates a single partition");
    }
}