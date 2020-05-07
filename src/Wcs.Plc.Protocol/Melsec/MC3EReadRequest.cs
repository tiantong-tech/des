using System;
using System.Globalization;

namespace Wcs.Plc.Protocol
{
  public class MC3EReadRequest: IPlcReadRequest
  {
    protected byte[] _msg = new byte[] {
      0x50,    // 0.  固定 - 帧头 - 1
      0x00,    // 1.  固定 - 帧头 - 2

      0x00,    // 2.  固定 - 网络编号
      0xFF,    // 3.  固定 - 可编程控制器编号
      0xFF,    // 4.  固定 - 请求目标模块IO编号 - 1
      0x03,    // 5.  固定 - 请求目标模块IO编号 - 2
      0x00,    // 6.  固定 - 请求目标模块站号

      0x00,    // 7.  参数 - 数据长度 - 1
      0x00,    // 8.  参数 - 数据长度 - 2

      0x00,    // 9.  参数 - 等待时间 (结束代码) - 1
      0x00,    // 10. 参数 - 等待时间 (结束代码) - 2

      0x00,    // 11. 参数 - 指令 - 1
      0x00,    // 12. 参数 - 指令 - 2
      0x00,    // 13. 参数 - 子指令 - 1
      0x00,    // 14. 参数 - 子指令 - 2

      0x00,    // 15. 参数 - 软元件地址 - 1
      0x00,    // 16. 参数 - 软元件地址 - 1
      0x00,    // 17. 参数 - 软元件地址 - 1
      0x00,    // 18. 参数 - 软元件代码

      0x00,    // 19. 参数 - 软元件点数 - 1
      0x00,    // 20. 参数 - 软元件点数 - 2

    };

    public byte[] Message { get => _msg; }

    protected bool IsWord = false;

    //

    private int TransAddressOffset(string offset)
    {
      var result = 0;
      var arr = offset.Split(".");

      result += int.Parse(arr[0]) * 8;

      if (arr.Length == 2) {
        result += int.Parse(arr[1]);
      }

      return result;
    }

    private int TransDbBlock(string block)
    {
      try {
        return int.Parse(block);
      } catch {
        throw new Exception("db block is invalid");
      }
    }

    private (string, int) TransAddress(string addr)
    {
      if (addr.Length < 2) {
        throw new Exception("address length is invalid");
      }

      var type = "D";
      var offset = 0;

      if (!int.TryParse(addr[1].ToString(), out _)) {
        type = addr.Substring(0, 2);
        addr = addr.Substring(2);
      } else if (!int.TryParse(addr[0].ToString(), out _)) {
        type = addr.Substring(0, 1);
        addr = addr.Substring(1);
      } else {
        throw new Exception("address type not found");
      }

      type = type.TrimEnd('*');
      if (type == "B" || type == "W") {
        offset = int.Parse(addr, NumberStyles.HexNumber);
      } else {
        offset = int.Parse(addr);
      }

      return (type, offset);
    }

    //

    protected void UseReadCommand()
    {
      Message[11] = 0x01;
      Message[12] = 0x04;
      Message[13] = 0x00;
      Message[14] = 0x00;
    }

    protected void UseWriteCommand()
    {
      Message[11] = 0x01;
      Message[12] = 0x14;
      Message[13] = 0x00;
      Message[14] = 0x00;
    }

    private void SetDataType(string type)
    {
      var (db, isWord) = type switch {
        "M" => (0x90, false),
        "V" => (0x94, false),
        "B" => (0xA0, false),
        "D" => (0xA8, true),
        "W" => (0xB4, true),
        _ => throw new Exception("data type is invalid")
      };

      Message[18] = (byte) db;
      IsWord = isWord;
    }

    private void SetDateOffset(int offset)
    {
      Message[15] = (byte)(offset % 256);               // 9.  偏移位置 - 1
      Message[16] = (byte)(offset / 256 % 256);         // 10. 偏移位置 - 2
      Message[17] = (byte)(offset / 256 / 256 % 256);   // 11. 偏移位置 - 3
    }

    private void SetDataLength(int length)
    {
      Message[19] = (byte)(length % 256);
      Message[20] = (byte)(length / 256);
    }
 
    protected void SetMessageLength()
    {
      Message[7] = (byte)((Message.Length - 9) % 256);
      Message[8] = (byte)((Message.Length - 9) / 256);
    }

    protected void SetAddress(string address)
    {
      var (db, offset) = TransAddress(address);

      SetDataType(db);
      SetDateOffset(offset);
    }

    //

    public void UseBool()
    {

    }

    public void UseUInt16()
    {

    }

    public void UseInt32()
    {

    }

    public void UseString(int length)
    {

    }

    public void UseBytes(int length)
    {

    }

    public void UseAddress(string address)
    {
      SetAddress(address);
      SetMessageLength();
      UseReadCommand();
    }

  }
}
