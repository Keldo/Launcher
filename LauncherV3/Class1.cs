using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherV3
{
    class Class1
    {
        internal string versionURL { get; set; }
        internal string wowBuild { get; set; }
        internal string launcherBuild { get; set; }
        internal string installURL { get; set; }
        internal string file1 { get; set; }
        internal string file2 { get; set; }
        internal string file3 { get; set; }
        internal string file4 { get; set; }
        internal string file5 { get; set; }
        internal string file6 { get; set; }
        internal string file7 { get; set; }

        public Class1()
        {
            // Game Version
            versionURL = "http://version.trinitywow.org/?version=";
            wowBuild = "wow";
            
            // Launcher Version
            launcherBuild = "launcher";

            // Game Files
            installURL = "http://www.trinitywow.org/game/install/legion/";
            file1 = "common.dll";
            file2 = "libeay32.dll";
            file3 = "libmysql.dll";
            file4 = "libssl32.dll";
            file5 = "ssleay32.dll";
            file6 = "connection_patcher.exe";
            file7 = "Wow.exe";

        }
    }
}
