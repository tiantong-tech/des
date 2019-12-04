using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class EventTest
  {
    // 使用 Use 安装插件
    static (Event, List<IEventArgs>) GetLoggedEvent()
    {
      var event_ = new Event();
      var plugin = new EventTestPlugin();
      event_.Use(plugin);

      return (event_, plugin.Logs);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(99)]
    public void TestEmit(int n)
    {
      var (event_, logs) = GetLoggedEvent();

      for (var i = 0; i < n; i++) {
        event_.Emit("any");
      }

      Assert.AreEqual(logs.Count, n);
    }

    [TestCase]
    public async Task TestEmitAsync()
    {
      var (event_, logs) = GetLoggedEvent();

      await event_.EmitAsync("any");

      Assert.AreEqual(logs.Count, 1);
    }

    // require deadlock
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(99)]
    public void TestOn(int n)
    {
      var times = 0;
      var (event_, logs) = GetLoggedEvent();

      for (var i = 0; i < n; i++) {
        event_.On("any", () => Interlocked.Increment(ref times));
      }
      event_.Emit("any");

      Assert.AreEqual(logs[0].HandlerCount, n);
      Assert.AreEqual(times, n);
    }

    // no deadlock
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(99)]
    public void TestOnEmit(int n)
    {
      var times = 0;
      var (event_, logs) = GetLoggedEvent();

      event_.On("any", () => times++);
      for (var i = 0; i < n; i++) {
        event_.Emit("any");
      }

      Assert.AreEqual(times, n);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(99)]
    public void TestOnce(int n)
    {
      var times = 0;
      var (event_, logs) = GetLoggedEvent();

      event_.Once("any", () => Interlocked.Increment(ref times));

      for (var i = 0; i < n; i++) {
        event_.Emit("any");
      }

      Assert.AreEqual(times, (n == 0 ? 0 : 1));
    }

    [TestCase]
    public void TestCancel()
    {
      var event_ = new Event();
      var listener = event_.On("any", () => Assert.Fail("Listener should be canceled yet"));
      listener.Cancel();

      event_.Emit("any");
      Assert.IsTrue(true);
    }

    [TestCase(1, "1")]
    [TestCase(3.14, "3.14")]
    [TestCase("helloworld", "\"helloworld\"")]
    public void TestEventArgsPayload<T>(T payload, string result)
    {
      var event_ = new Event();

      event_.All(args => Assert.AreEqual(args.Payload, result));
      event_.Emit("args", payload);
    }

    [Test]
    public void TestEventArgsNull()
    {
      var event_ = new Event();

      event_.All(args => Assert.AreEqual(args.Payload, "null"));
      event_.On<string>("null", value => Assert.AreEqual(value, null));
      event_.On<object>("null", value => Assert.AreEqual(value, null));
      event_.On<Nullable<int>>("null", value => Assert.AreEqual(value, null));
      event_.Emit<string>("null", null);
      event_.Emit<object>("null", null);
      event_.Emit<Nullable<int>>("null", null);
    }

    [Test]
    public void TestOnCustom()
    {
      var event_ = new Event();
      var obj = new { Id = 10, Name = "yuanfang" };
      var user = new EventUser { Id = 10, Name = "yuanfang" };

      event_.On<EventUser>("obj", data => {
        Assert.AreEqual(data.Id, 10);
        Assert.AreEqual(data.Name, "yuanfang");
      });
      event_.On<List<EventUser>>("objs", list => {
        list.ForEach(obj => {
            Assert.AreEqual(obj.Id, 10);
            Assert.AreEqual(obj.Name, "yuanfang");
        });
      });

      event_.Emit("obj", obj);
      event_.Emit("obj", user);
      event_.Emit("objs", new List<EventUser> { user, user, user, user });
    }

    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue)]
    [TestCase(Int64.MaxValue)]
    [TestCase(Int64.MinValue)]
    [TestCase(double.MaxValue)]
    [TestCase(double.MinValue)]
    [TestCase(true)]
    [TestCase(false)]
    [TestCase("")]
    [TestCase("happy hacking")]
    public void TestEventType<T>(T payload)
    {
      var event_ = new Event();

      event_.On<T>("any", value => Assert.AreEqual(value, payload));
      event_.Emit("any", payload);
    }

    [TestCase(1, 1, 1)]
    [TestCase(1, 2, 3, "a", "b", "c", true, false)]
    public void TestEventArr(params object[] arr)
    {
      var event_ = new Event();
      event_.On<object[]>("arr", values => {
        for (var i = 0; i < arr.Length; i++) {
          Assert.AreEqual(values[i].ToString(), arr[i].ToString());
        }
      });
      event_.Emit("arr", arr);
    }
  }
}
