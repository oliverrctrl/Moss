using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.ExtendedASCII;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics.Fonts;
using Moss.Core.Graphics;
using Moss.Core.Interfaces;
using Moss.Core.Shell;
using Sys = Cosmos.System;

namespace Moss
{
    public class Kernel : Cosmos.System.Kernel
    {
        public const string Version = "0.0.1";

        public static CosmosVFS Vfs { get; private set;  }
        
        public static Interface Interface { get; private set;  }
        public List<ICoreScript> CoreScripts { get; private set;  }
        
        protected override void BeforeRun()
        {
            try
            {
                Vfs = new CosmosVFS();
                VFSManager.RegisterVFS(Vfs);

                CoreScripts = new List<ICoreScript>();
                Interface =  new Interface();


                foreach (var cs in CoreScripts)
                {
                    Console.WriteLine($"Starting {cs}");
                    cs.Start();
                }

                Console.OutputEncoding = CosmosEncodingProvider.Instance.GetEncoding(437)!;
                
                
                Interface.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void Run()
        {
            
            foreach (var cs in CoreScripts)
            {
                cs.Update();
            }
            Interface.Update();
        }
        
    }
}