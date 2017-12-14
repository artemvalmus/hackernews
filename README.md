
## Running

Open HackerNews folder in cmd and run the following command (it is assumed you have .NET Core installed. If not you can download it here   https://www.microsoft.com/net/download/windows):

```
dotnet publish -c Release -r <runtime>
```

Where `runtime` is the runtime to build for (e.g. win10-x64, ubuntu.16.10-x64)

Open the output folder in cmd and run:

```
hackernews --posts <n>
```
Where `n` is the maximum number of posts to print (positive integer <= 100)

## Libraries

`Newtonsoft.Json` - JSON framework for .NET which provides an ability to serialize and deserialize JSON

`Mono.Options` - option parsing library for C# which helps to parse input parameters
