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

### 6. ASPNET 读取配置文件的顺序

```bash
appsetting.json -> appsetting.Development.json -> Secrets.json -> Environment Variables -> Command Line Args

```

### 7.使用 Secret 管理隐私数据

为什么要使用 Secret 呢，本地开发存放隐私数据在本地，不需要写在配置文件里面，避免提交的 git 上

Secret 会在用户目录上生成一个 secrets.json 的文件存放我的隐私数据，然后本地开发程序可以通过 Configuration 读取到

1.首先要在 项目的 csproj 文件的 PropertyGroup 里配置一个全局唯一序列号，方便程序加载的时候能够快速找到该文件，全局唯一序列号 UUID 可以用 VSCode 插件生成

如：

```xml
<UserSecretsId>5ad6f641-e6c6-4cc7-be03-19db479916fd</UserSecretsId>
```

2.设置要保存到本地的 Secret 的数据

```bash
$ dotnet user-secrets set "UserID" "你的数据库用户名"

$ dotnet user-secrets set "Password" "你的数据库用户的密码"
```

3.可以在以下目录看到生成的 secrets.json 文件(非加密文件)，user_secrets_id 就是你在上面设置的 UUID

- Windows: %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\
  secrets.json
- Linux/OSX: ~/.microsoft/usersecrets/<user_secrets_id>/
  secrets.json
