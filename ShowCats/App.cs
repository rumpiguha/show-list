using Microsoft.Extensions.Logging;
using ShowCats.Models.Enum;
using ShowPets.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowPets
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IFilterService _filterService;
        private readonly TextWriter _outputWriter;
        private readonly TextReader _inputReader;
        public const string PressAnyKey = "\nPress any key to continue.";

        public App(ILogger<App> logger, IFilterService filterService, TextWriter outputWriter, TextReader inputReader)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._filterService = filterService ?? throw new ArgumentNullException(nameof(filterService));
            this._outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
            this._inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
        }

        public async Task RunAsync()
        {
            var result = await _filterService.FilterPets();

            if (!result.ContainsKey("Error"))
            {
                foreach (KeyValuePair<string, List<string>> entry in result)
                {
                    _outputWriter.WriteLine(entry.Key);
                    _outputWriter.WriteLine();

                    //Sort Names Alphabetically
                    entry.Value.Sort();
                    entry.Value.ForEach(petName => _outputWriter.WriteLine($" * {petName}"));
                    _outputWriter.WriteLine();
                }
            }
            else
            {
                _outputWriter.WriteLine($"Error: {result["Error"][0]}");
            }
            _outputWriter.WriteLine(PressAnyKey);
            _inputReader.Read();
        }
    }
}
