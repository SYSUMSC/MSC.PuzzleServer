# Log API

## Get logs

- PageUrl: `/Admin/Log`
- APIUrl: `/api/admin/log`
- Params:
    - Skip: 跳过数量，默认为0
    - Count: 获取日志数量，默认为最大值50
    - Level: 日志等级，可以为`Info, Warning, Error, Fatal, Debug`等，使用`All`获取全部。
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 获取成功，数据见`data`字段

## LogHub with SignalR

库：

```html
<script src="https://cdn.jsdelivr.net/npm/@@microsoft/signalr@5.0.3/dist/browser/signalr.min.js"></script>
```

主体代码示例：

```js
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/log")
    .withAutomaticReconnect()
    .build();

connection.on("RecivedLog", (data) => {
    console.log(data)
    $("<tr><th scope='row'>" + data.time + "</th><td>" + data.name + "</td><td>" + data.ip + "</td><td>" + data.msg + "</td><td>" + data.status + "</td></tr>").hide().prependTo("#logs").fadeIn(200)
});

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(start);

start();
```
