# Innergy.Demo

## Build status

[![mattuu](https://circleci.com/gh/mattuu/Innergy.Demo.svg?style=svg)](https://app.circleci.com/pipelines/github/mattuu/Innergy.Demo)

[![codecov](https://codecov.io/gh/mattuu/Innergy.Demo/branch/master/graph/badge.svg)](https://codecov.io/gh/mattuu/Innergy.Demo)

## Instructions

```
cd src\Innergy.Demo.Console
dotnet run [-- <input_file_path> <output_file_path>]
```

> `<input_file_path>` argument is optional. If not specified, default file path for input file is `\tmp\input.txt`.
> `<output_file_path>` argument is optional. If not specified, output will be written to console.

### Sample Data

There is a sample data file available in repo: `data\input.txt`. Sample data file contains data provided in task description.

> It's necessary to copy `input.txt` into `\tmp` folder before starting the application.

