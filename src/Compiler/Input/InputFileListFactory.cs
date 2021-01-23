﻿using System.Collections.Generic;
using Compiler.Config;
using Compiler.Output;

namespace Compiler.Input
{
    public class InputFileListFactory
    {
        public static InputFileList CreateFromInclusionRules(
            SectorDataFileFactory dataFileFactory,
            ConfigInclusionRules config,
            OutputGroupRepository outputGroups
        ) {
            InputFileList fileList = new InputFileList();

            foreach (IInclusionRule rule in config)
            {
                OutputGroup group = rule.GetOutputGroup();
                List<string> filePaths = new();
                foreach (AbstractSectorDataFile file in rule.GetFilesToInclude(dataFileFactory))
                {
                    fileList.Add(file);
                    filePaths.Add(file.FullPath);
                }

                outputGroups.AddGroupWithFiles(group, filePaths);
            }
            return fileList;
        }
    }
}
