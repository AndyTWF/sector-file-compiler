﻿using Compiler.Output;
using Newtonsoft.Json;

namespace Compiler.Argument
{
    public class CompilerArgumentsValidator
    {
        private readonly ILoggerInterface logger;
        public CompilerArgumentsValidator(ILoggerInterface logger)
        {
            this.logger = logger;
        }

        public bool Validate(CompilerArguments arguments)
        {
            bool valid = true;
            if (!arguments.ConfigFile.Exists())
            {
                valid = false;
                this.logger.Error("The configuration file could not be found: " + arguments.ConfigFile.GetPath());
            }

            if (arguments.ConfigFile.Exists())
            {
                try
                {
                    JsonConvert.DeserializeObject(arguments.ConfigFile.Contents());
                }
                catch
                {
                    valid = false;
                    this.logger.Error("The configuration file is not valid JSON");
                }
            }

            if (!arguments.OutFile.IsWritable())
            {
                valid = false;
                this.logger.Error("The output file is not writable: " + arguments.OutFile.GetPath());
            }

            return valid;

        }
    }
}
