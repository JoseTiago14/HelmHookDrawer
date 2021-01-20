using System;
using System.Linq;
using Flow.Host;
using json.parser;
using Microsoft.Extensions.DependencyInjection;

namespace console.runner
{

    class Program
    {

        static void Main(string[] args)
        {
            var json = "undefined"; //TODO eventually pass as argument(NOTE replace with the json you want to parse)

            new Host().Run(sp => {
                Console.WriteLine("\n\nTree\n");
                var tree = sp.GetService<TreeParser>();

                var treeItems = tree.Parse(json);

                treeItems.ToList()
                         .ForEach(item => Console.WriteLine(item.Kind));

                Console.WriteLine("\n\nList\n");
                var list = sp.GetService<ListParser>();

                var listItems = list.Parse(json);

                listItems.ToList()
                         .ForEach(item => Console.WriteLine(item.Kind));
            });
        }

    }

}
