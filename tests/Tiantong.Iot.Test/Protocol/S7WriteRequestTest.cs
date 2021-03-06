// using NUnit.Framework;

// namespace Tiantong.Iot.Protocol.Test
// {
//   public class S7WriteRequestTest
//   {
//     [Test]
//     public void test_d_1_100_int_read_write()
//     {
//       var write = new [] {
//         0x03, 0x00, 0x00, 0x27, 0x02, 0xF0, 0x80,
//         0x32, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x08, 0x05, 0x01,
//         0x12, 0x0A, 0x10, 0x02, 0x00, 0x04, 0x00, 0x01, 0x84, 0x00, 0x03, 0x20,
//         0x00, 0x04, 0x00, 0x20, 0x00, 0x00, 0x00, 0x64
//       };
//       var req = new S7WriteRequest();

//       req.UseUInt16();
//       req.UseAddress("D1.100");
//       req.UseData((ushort) 100);

//       Assert.AreEqual(req.Message, write);
//     }

//     // [Test]
//     // public void test_d_1_100_bool_read_write()
//     // {
//     //   var write = new [] {
//     //     0x03, 0x00, 0x00, 0x24, 0x02, 0xF0, 0x80,
//     //     0x32, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x05, 0x05, 0x01,
//     //     0x12, 0x0A, 0x10, 0x02, 0x00, 0x01, 0x00, 0x01, 0x84, 0x00, 0x03, 0x21,
//     //     0x00, 0x04, 0x00, 0x08, 0x01
//     //   };
//     //   var req = new S7WriteRequest();

//     //   req.UseAddress("D1.100.1");
//     //   req.UseData(true);

//     //   Assert.AreEqual(req.Message, write);
//     // }

//     [Test]
//     public void test_d_1_100_string_read_write()
//     {
//       var write = new [] {
//         0x03, 0x00, 0x00, 0x2D, 0x02, 0xF0, 0x80,
//         0x32, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x0E, 0x05,
//         0x01, 0x12, 0x0A, 0x10, 0x02, 0x00, 0x0A, 0x00, 0x01, 0x84, 0x00, 0x03, 0x20,
//         0x00, 0x04, 0x00, 0x50, 0x09, 0x31, 0x32, 0x33, 0x34, 0x35, 0x00, 0x00, 0x00, 0x00
//       };
//       var req = new S7WriteRequest();
      
//       req.UseString(10);
//       req.UseAddress("D1.100");
//       req.UseData("12345");

//       Assert.AreEqual(req.Message, write);
//     }

//     [Test]
//     public void test_overlength_read_write()
//     {
//       var write = new [] {
//         0x03, 0x00, 0x00, 0x2D, 0x02, 0xF0, 0x80,
//         0x32, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x0E, 0x05,
//         0x01, 0x12, 0x0A, 0x10, 0x02, 0x00, 0x0A, 0x00, 0x01, 0x84, 0x00, 0x03, 0x20,
//         0x00, 0x04, 0x00, 0x50, 0x09, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39
//       };
//       var req = new S7WriteRequest();

//       req.UseString(10);
//       req.UseAddress("D1.100");
//       req.UseData("123456789012345");

//       Assert.AreEqual(req.Message, write);
//     }
//   }
// }
