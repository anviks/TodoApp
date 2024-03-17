```bash
dotnet tool update --global dotnet-ef
```

```bash
dotnet ef migrations --project DAL --startup-project ConsoleApp add initial
```

```bash
dotnet ef migrations --project DAL --startup-project ConsoleApp remove
```

```bash
dotnet ef database --project DAL --startup-project ConsoleApp update
```

```bash
dotnet ef database --project DAL --startup-project ConsoleApp drop
```

```bash
cd WebApp

dotnet aspnet-codegenerator razorpage `
    -m Domain.Recipe `
    -dc AppDbContext `
    -udl `
    -outDir Pages/Recipes `
    --referenceScriptLibraries
```

* `-m` - Name of the model class
* `-dc` - Data context class
* `-udl` - Use default layout
* `-outDir` - Where to generate the output
* `--referenceScriptLibraries` - Add validation scripts on Edit and Create pages