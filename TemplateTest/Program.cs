// See https://aka.ms/new-console-template for more information


using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;

Console.WriteLine("Hello, World!");
var aa = new[] { 1, 11, 4, 7, 2, 3, 88, 35, 4 };
Sort(aa);
Console.ReadLine();
static void Sort(int[] nums)
{
int temp;
for (int i = 0; i < nums.Length; i++)
{
for (int j = i + 1; j < nums.Length; j++)
{
if (nums[i] > nums[j])
{
temp = nums[i];
nums[i] = nums[j];
nums[j] = temp;
}
}
Console.WriteLine(nums[i]);
}
}

//简单使用
string template = "Hello @Model.Name,客户 @Model.Subj" +
    "ect 退件通知, welcome to RazorEngine!";
var a = JsonConvert.DeserializeObject("{\"Name\":\"Marty\",\"Subject\":\"包裹\",\"age\":\"123\",\"sss\":123}");
var result = Engine.Razor.RunCompile(template, "templateKey", null, a);
Console.WriteLine(result);