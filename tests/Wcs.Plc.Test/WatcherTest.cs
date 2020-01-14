using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class WatcherTest
  {
    [Test]
    public void TestHandleCancel()
    {
      var result = 0;
      var event_ = new Event();
      var watcher = new Watcher<int>(event_);
      var listener = event_.On<int>("event", value => result = value);

      watcher.When(value => value != 0).Event("event");
      watcher.Handle(1);
      Assert.AreEqual(result, 1);

      watcher.OnCancel(() => listener.Cancel());
      watcher.Cancel();
      watcher.Handle(2);
      Assert.AreEqual(result, 1);
    }

    [Test]
    public void TestCustomEvent()
    {
      var result = 0;
      var event_ = new Event();
      var watcher = new Watcher<EventUser>(event_);
      var user = new EventUser() { Id = 1, Name = "test" };

      event_.On<EventUser>("event", item => result = item.Id);
      watcher.When(item => item.Id == 10).Event("event");

      watcher.Handle(user);
      Assert.AreEqual(result, 0);

      user.Id = 10;
      watcher.Handle(user);
      Assert.AreEqual(result, 10);
    }

    [Test]
    public void TestCompare()
    {
      var result = 0;
      var event_ = new Event();
      var watcher = new ComparableWatcher<int>(event_);
      var listener = event_.On<int>("event", value => result = value);

      watcher.Event("event");

      watcher.When("!=", 0).Handle(1);
      Assert.AreEqual(result, 1);

      watcher.When("==", 0).Handle(0);
      Assert.AreEqual(result, 0);

      watcher.When(">", 0).Handle(10);
      Assert.AreEqual(result, 10);

      watcher.When(">=", 0).Handle(0);
      Assert.AreEqual(result, 0);

      watcher.When("<", 0).Handle(-10);
      Assert.AreEqual(result, -10);

      watcher.When("<=", 0).Handle(0);
      Assert.AreEqual(result, 0);

      watcher.When("??", 1).Handle(1);
      Assert.AreEqual(result, 1);
    }
  }
}
