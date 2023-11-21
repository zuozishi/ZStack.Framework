# ZStack快速开发框架

## ZStack.Extensions【ZStack框架 拓展方法库】

### DateTimeExtension【日期时间拓展方法】

```c#
// 获取时间戳（秒）
DateTime.Now.ToTimestampSeconds();

// 获取时间戳（毫秒）
DateTime.Now.ToTimestampMilliseconds();
```

### StringExtension【字符串拓展方法】

```c#
// 获取MD5
"123".GetMD5();

// 将字符串URL编码
"张三".UrlEncode(); // %E5%BC%A0%E4%B8%89

// 将字符串URL解码
"%E5%BC%A0%E4%B8%89".UrlDecode(); // 张三

// JSON字符串转对象
"{\"Name\":\"张三\"}".ToObject<User>();
"{\"Name\":\"张三\"}".ToObject<User>(JsonSerializerOptions serializerOptions); // 自定义序列化配置

// JSON字符串转JsonObject
"{\"Name\":\"张三\"}".ToJsonObject();
"{\"Name\":\"张三\"}".TryToJsonObject(out var jsonObject);

// JSON字符串转JsonArray
"[1,2,3]".ToJsonArray();
"[1,2,3]".TryToJsonArray(out var jsonArray);

// JSON字符串转JsonValue
"true".ToJsonValue();
"true".TryToJsonValue(out var jsonValue);

// 验证是否为有效的手机号
"12345678901".IsMobileNumber();

// 验证是否为有效的邮箱
"12345678901@qq.com".IsEmail();

// 验证是否为有效的身份证号
"12345678901".IsIdCardNumber();
```

### GuidExtension【Guid拓展方法】

```c#
// 获取32位长度的Guid字符串
Guid.NewGuid().ToUUID32();
```

### IEnumerableExtension【集合拓展方法】

```c#
// 遍历集合
var list = new List<int> { 1, 2, 3 };
list.ForEach(item => Console.WriteLine(item));

// 遍历集合（异步）
var list = new List<int> { 1, 2, 3 };
await list.ForEachAsync(async item => {
    // 异步操作
});

// 集合转分隔符字符串
var list = new List<int> { 1, 2, 3 };
list.JoinToString(",");

// 将字典转化为QueryString格式
var dict = new Dictionary<string, string> {
    { "Name", "张三" }, { "Age", "18" }
};
dict.ToQueryString(); // Name=张三&Age=18
```

### ObjectExtension【对象扩展方法】

```c#
// 对比对象属性值差异
var user1 = new { Name = "张三", Age = 18 };
var user2 = new { Name = "李四", Age = 20 };
var diffList = user1.Diff(user2);

// 对象转JSON字符串
var user = new { Name = "张三", Age = 18 };
user.ToJsonString(bool indented = true); // 默认缩进
user.ToJsonString(JsonSerializerOptions serializerOptions); // 自定义序列化配置

// 判断类型是否实现某个泛型
typeof(List<int>).IsImplementFromGeneric(typeof(IEnumerable<>));

// 将object转换为long，若失败则返回0或自定义值
"123".ToLongOrDefault(); // 123
"abc".ToLongOrDefault(); // 0
"abc".ToLongOrDefault(1); // 1

// 将object转换为double，若失败则返回0或自定义值
"123".ToDoubleOrDefault(); // 123
"abc".ToDoubleOrDefault(); // 0
"abc".ToDoubleOrDefault(1); // 1

// 将string转换为DateTime，若失败则返回日期最小值或自定义值
"2021-01-01".ToDateTimeOrDefault(); // 2021-01-01 00:00:00
"abc".ToDateTimeOrDefault(); // 0001-01-01 00:00:00
"abc".ToDateTimeOrDefault(DateTime.Now); // 当前时间

// 判断是否有值
"".IsNullOrEmpty(); // true
"abc".IsNullOrEmpty(); // false
```