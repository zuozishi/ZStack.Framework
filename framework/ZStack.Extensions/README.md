# ZStack.Extensions

ZStack框架扩展方法库，提供常用的C#扩展方法和工具类。

## DateTime 扩展

```csharp
// 获取Unix时间戳（秒）
DateTime.Now.ToTimestampSeconds();

// 获取Unix时间戳（毫秒）
DateTime.Now.ToTimestampMilliseconds();
```

## String 扩展

```csharp
// MD5加密
"123".GetMD5();

// URL编码/解码
"张三".UrlEncode();  // %E5%BC%A0%E4%B8%89
"%E5%BC%A0%E4%B8%89".UrlDecode();  // 张三

// JSON字符串转对象
"{\"Name\":\"张三\"}".ToObject<User>();
"{\"Name\":\"张三\"}".ToObject<User>(jsonSerializerOptions);

// JSON字符串转JsonObject / JsonArray / JsonValue
"{\"Name\":\"张三\"}".ToJsonObject();
"{\"Name\":\"张三\"}".TryToJsonObject(out var jsonObject);
"[1,2,3]".ToJsonArray();
"[1,2,3]".TryToJsonArray(out var jsonArray);
"true".ToJsonValue();
"true".TryToJsonValue(out var jsonValue);

// 数据验证
"13800138000".IsMobileNumber();
"test@example.com".IsEmail();
"110101199001011234".IsIdCardNumber();
```

## Guid 扩展

```csharp
// 获取32位无连字符的Guid字符串
Guid.NewGuid().ToUUID32();
```

## IEnumerable 扩展

```csharp
// 遍历集合
list.ForEach(item => Console.WriteLine(item));

// 异步遍历集合
await list.ForEachAsync(async item => { /* 异步操作 */ });

// 集合转分隔符字符串
list.JoinToString(",");

// 字典转QueryString
new Dictionary<string, string> { {"Name", "张三"}, {"Age", "18"} }
    .ToQueryString();  // Name=张三&Age=18
```

## Object 扩展

```csharp
// 对比两个对象属性差异
var diffs = user1.Diff(user2);

// 对象转JSON字符串
user.ToJsonString(indented: true);
user.ToJsonString(jsonSerializerOptions);

// 类型判断
typeof(List<int>).IsImplementFromGeneric(typeof(IEnumerable<>));

// 安全类型转换（转换失败返回默认值）
"123".ToLongOrDefault();   // 123
"abc".ToLongOrDefault(1);  // 1
"123.45".ToDoubleOrDefault();
"2021-01-01".ToDateTimeOrDefault();

// 空值判断
"".IsNullOrEmpty();
"abc".IsNullOrEmpty();
```

## Stream 扩展

```csharp
// Stream 转 Byte[]
stream.ToArray();
```

## Number 扩展

```csharp
// 数值转千分位字符串
1234567.ToNString();  // "1,234,567.00"
```

## Reflection 扩展

```csharp
// 获取异步方法真实返回类型
method.GetRealReturnType();

// 判断方法是否是异步方法
method.IsAsync();
```

## 工具类

### DataValidator

```csharp
DataValidator.IsValidMobile("13800138000");
DataValidator.IsValidEmail("test@example.com");
DataValidator.IsValidIdCard("110101199001011234");
```

### MD5

```csharp
MD5.GetMD5(string input);
MD5.GetMD5(byte[] bytes);
MD5.GetMD5(Stream stream);
```

### DataUtils

```csharp
DataUtils.IsEqual(value1, value2);
```

### ValueDiffList

对象属性差异比较结果集合，由 `Diff()` 方法返回。
