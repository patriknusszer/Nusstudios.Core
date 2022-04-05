using System.IO;
using System.Collections.Generic;

namespace Nusstudios.Core
{
    public class IO
    {
        public delegate bool WalkCallback(string node, string subNode, WalkReportType wrt);

        public enum WalkReportType
        {
            ForwardsWalk,
            BackwardsWalk,
            Root
        }

        public static bool IsHidden(string node) => Path.GetFileName(node)[0].Equals('.');

        public static bool Walk(string node, WalkCallback cb)
        {
            if (Directory.Exists(node))
            {
                List<string> subNodes = new List<string>();
                subNodes.AddRange(Directory.GetFiles(node, "*.*", SearchOption.TopDirectoryOnly));
                subNodes.AddRange(Directory.GetDirectories(node, "*", SearchOption.TopDirectoryOnly));

                if (subNodes.Count == 0)
                {
                    return cb(node, null, WalkReportType.Root);
                }
                else
                {
                    foreach (string subNode in subNodes)
                    {
                        if (cb(node, subNode, WalkReportType.ForwardsWalk))
                            return true;
                        if (Walk(subNode, cb))
                            return true;
                        if (cb(node, subNode, WalkReportType.BackwardsWalk))
                            return true;
                    }

                    return false;
                }
            }
            else if (File.Exists(node))
            {
                return cb(node, null, WalkReportType.Root);
            }
            else {
                return false;
            }
        }
    }
}
