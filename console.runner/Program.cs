using System;
using System.IO;
using System.Linq;
using Flow.Host;
using Microsoft.Extensions.DependencyInjection;
using yaml.parser;
using Parser = yaml.parser.Parser;

namespace console.runner
{

    class Program
    {

        static int Main(string[] args) =>
            new Host().Run<Options>(args,(sp, opts) =>
            {
                var fileInfo = new FileInfo(opts.Path);
                if (!fileInfo.Exists)
                {
                    Console.WriteLine($"Helm template file not found at {args[1]}");
                    return 1;
                }

                var yaml = File.ReadAllText(fileInfo.FullName);

                var tree = sp.GetService<Parser>();
                var treeItems = tree.Parse(yaml, opts.Mode);

                treeItems[Stage.Pre].ToList().ForEach(item =>
                    Console.WriteLine(
                        $"[Pre {opts.Mode}]\tChart: {item.ChartName} | Kind: {item.Kind} | Resource: {item.Name} | Namespace: {item.Namespace} | Weight: {item.Weight}"));
                treeItems[Stage.Pre].ToList().ForEach(item =>
                    Console.WriteLine(
                        $"[{opts.Mode}]\tChart: {item.ChartName} | Kind: {item.Kind} | Resource: {item.Name} | Namespace: {item.Namespace}"));
                treeItems[Stage.Post].ToList().ForEach(item =>
                    Console.WriteLine(
                        $"[Post {opts.Mode}]\tChart: {item.ChartName} | Kind: {item.Kind} | Resource: {item.Name} | Namespace: {item.Namespace} | Weight: {item.Weight}"));
                return 0;
            }, (provider, errors, arg3) =>
            {
                Console.WriteLine($"Failed unexpectedly to parse helm template output:\n{string.Join("\n",errors.Select(e=>e.ToString()))}");
                return -1;
            });
    }
}
