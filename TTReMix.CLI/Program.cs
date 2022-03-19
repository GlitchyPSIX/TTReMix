using System;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using static TTReMix.TTRMTools;
namespace TTReMix.CLI {

    class Program {
        static void Main(string[] args) {
            string output = null;
            if (args.Length < 1) {
                DisplayHelp();
                return;
            }

            if (args.Length >= 4) {
                output = args[3];
            }

            switch (args[0]) {
                case "-m": {
                        if (args.Length < 3) {
                            Console.WriteLine("\nEither start replay path or end replay path not passed as arguments.\nUse ttremix -h for help.");
                            break;
                        }

                        Remix(args[1], args[2], false, output);
                        break;
                    }
                case "-s": {
                        if (args.Length < 3) {
                            Console.WriteLine("\nEither start replay path or end replay path not passed as arguments.\nUse ttremix -h for help.");
                            break;
                        }

                        Remix(args[1], args[2], true, output);
                        return;
                    }
                case "-h": {
                        DisplayHelp();
                        return;
                    }
            }
        }

        static void Remix(string path1, string path2, bool stitch, string output = null) {
            if (!File.Exists(path1) || !File.Exists(path2)) {
                Console.WriteLine("\nOne of the replay files cannot be found.");
                return;
            }

            try {
                string ttrmJson = MergeTTRMPath(path1, path2, stitch);
                string outputPath = Path.GetFullPath(output ?? "merged.ttrm");


                if (File.Exists(outputPath)) {
                    Console.Write($"\nThere's a replay here ({outputPath}) with the same name. Overwrite? (Press \"y\" for yes, any other key for no) ");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key.ToString().ToLower() != "y")
                    {
                        return;
                    }
                    File.Delete(outputPath);
                }

                File.WriteAllText(outputPath, ttrmJson);
                Console.WriteLine($"\nTTRM merged to {outputPath}");
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }

        static void DisplayHelp() {
            Console.Write(
@"
TTReMix - Multiplayer Merger for TETR.IO Replay Data
May break. Use with caution.
-- GlitchyPSI
-- TETR.IO by osk

Usage: ttrmx [-OPTION] <start> <end> [output]
    
    <start>     The path to the first replay

    <end>       The path to the replay file with the matches that will be
                added to the end of the merged file

    [output]    Optional. The path at which the merged file will be saved
                Defaults to <application exe>/merged.ttrm
    
    Any existing file with the same filename will be asked before being
    overwritten.

    Possible values for OPTION:
        -s      Stitch replays
                Removes the last match from the first replay and reduces the
                winner's wincount by one.
                
                Use only when merging replays with a disconnect inbetween.

        -m      Merge replays
                Just adds both replays together.

        -h      Displays this help");
        }
    }
}
