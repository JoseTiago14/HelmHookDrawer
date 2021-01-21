using System;
using System.IO;
using System.Linq;
using Flow.Host;
using Microsoft.Extensions.DependencyInjection;
using yaml.parser;

namespace console.runner
{

    class Program
    {

        static void Main(string[] args)
        {
            var yaml = File.ReadAllText(@"C:\Users\pereiraj\Downloads\output.yaml"); //TODO eventually pass as argument(NOTE replace with the json you want to parse)
            var mode = ChartMode.Install;

            new Host().Run(sp => {
                //Console.WriteLine("\n\nTree\n");
                //var tree = sp.GetService<TreeParser>();

                //var treeItems = tree.Parse(yaml, mode);

                //treeItems.ToList()
                //         .ForEach(item => Console.WriteLine(item.Kind));

                Console.WriteLine("\n\nList\n");
                var list = sp.GetService<ListParser>();

                var listItems = list.Parse(yaml, mode);

                listItems.ToList()
                         .ForEach(item => Console.WriteLine(item.Kind));
            });
        }

    }

}
