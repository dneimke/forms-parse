
using FormsParse;

string config = string.Format("a{0}b{0}--{0}c", Environment.NewLine);
//Runner.Run(config, 2, 3);

config = string.Format("a{0}b{0}--{0}c--{0}d--", Environment.NewLine);
//Runner.Run(config, 2, 4);

config = string.Format("a{0}b{0}--{0}c{0}--{0}d--", Environment.NewLine);
//Runner.Run(config, 3, 4);

config = string.Format("#(a, Red){0}b{0}", Environment.NewLine);
//Runner.Run(config, 1, 2, true);

config = string.Format("#(a, Red){0}b{0}", Environment.NewLine);
//Runner.Run(config, 1, 2, true);

config = String.Format("#(a, Red){0}#(b, Green, Label){0}|{0}#(c, Blue){0}#(d, Blue){0}--{0}e{0}f", Environment.NewLine); // (name, [color, type])
Runner.Run(config, 2, 6, true);

Console.ReadKey();



static class Runner
{
    internal static void Run(string config, int expectedGroups, int expectedItems, bool showDebug = false)
    {
        var parser = new FormParser();
        var def = parser.Parse(config);

        Console.WriteLine($"Processing {config}");
        Console.WriteLine($"Config contains {def.RowCount} group(s) vs. {expectedGroups} expected.");
        Console.WriteLine($"Config contains {def.TotalItemCount} item(s) vs {expectedItems} expected.");

        var multiColumns = def.Rows.Where(x => x.Columns.Count > 1);
        for(int i = 0; i < multiColumns.Count(); i++)
        {
            var group = multiColumns.Skip(i).Take(1).Single();
            Console.WriteLine($"Row {i + 1} has {group.Columns.Count} columns.");
        }


        if(showDebug)
        {
            Console.WriteLine("--------------------------");

            for (int i = 0; i < def.AllItems.Count; i++)
            {
                var item = def.AllItems[i];
                Console.WriteLine($"Item {i + 1}: Name: {item.Name}, Color: {item.Color}, Type: {item.Type}");
            }
        }

        Console.WriteLine("--------------------------");
    }
}