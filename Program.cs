using System.CommandLine;
using System.Security.Cryptography;


// 創建命令選項
var rsaOption = new Option<bool>(
    name: "--rsa",
    description: "生成 RSA 金鑰對");

var aesOption = new Option<bool>(
    name: "--aes",
    description: "生成 AES 金鑰");

var outputOption = new Option<string>(
    name: "--output",
    description: "輸出文件名稱",
    getDefaultValue: () => "key");
    
// 創建根命令
var rootCommand = new RootCommand("金鑰生成工具")
{
    rsaOption,
    aesOption,
    outputOption,
};


// 設置命令處理程序
rootCommand.SetHandler((rsa, aes, output) =>
{
    try
    {
        if (rsa && aes)
        {
            Console.WriteLine("錯誤: 不能同時指定 --rsa 和 --aes");
        }
        else if (!rsa && !aes)
        {
            Console.WriteLine("錯誤: 必須指定 --rsa 或 --aes");
        }
        else if (rsa)
        {
            GenerateRsaKeys(output);
            Console.WriteLine($"已生成 RSA 金鑰: {output}");
        }
        else if (aes)
        {
            GenerateAesKey(output);
            Console.WriteLine($"已生成 AES 金鑰: {output}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"錯誤: {ex.Message}");
    }
    
}, rsaOption, aesOption, outputOption);


return rootCommand.Invoke(args);

// 生成 RSA 金鑰對
static void GenerateRsaKeys(string baseName)
{
    var rsaKey = RSA.Create();
    var privateKey = rsaKey.ExportRSAPrivateKey();
    File.WriteAllBytes(baseName, privateKey);
}

// 生成 AES 金鑰
static void GenerateAesKey(string baseName)
{
    byte[] key = new byte[32];
    RandomNumberGenerator.Fill(key);
    File.WriteAllBytes(baseName, key);
}