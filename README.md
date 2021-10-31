# CommandAPISolution

.NET Core API Demo

### 1.创建目录结构

```bash
$ mkdir CommandAPISolution
$ cd CommandAPISolution
$ mkdir src
$ mkdir test
```

这里使用 sln 的方式，sln 可以集成多个 project，src 放 Source Code，test 放 Unit test

### 2.创建项目

```bash
$ cd src
$ dotnet new web -n CommandAPI # 这里使用web是想手动敲代码，实际开发中可一直接用webapi生成代码

$ cd test
$ dotnet new xunit -n CommandAPI.Tests
```

### 3.创建 Solution 关联 Project

```bash
$ cd ../
$ dotnet new sln --name CommandAPISolution

$ dotnet sln CommandAPISolution.sln add src/CommandAPI/CommandAPI.csproj test/CommandAPI.Tests/CommandAPI.Tests.csproj
```

### 4.关联 Source Code 和 Unit Test

推荐使用命令行

```bash
$ dotnet add test/CommandAPI.Tests/CommandAPI.Tests.csproj reference src/CommandAPI/CommandAPI.csproj
```

或者直接在 test/CommandAPT.Tests/CommandAPI.Tests.csproj 文件中添加

```xml
<ItemGroup>
  <ProjectReference Include="..\..\src\CommandAPI\CommandAPI.csproj" />
</ItemGroup>
```

### 5.编译 sln 是否正确运行, 根目录下运行

```bash
$ dotnet build
```
