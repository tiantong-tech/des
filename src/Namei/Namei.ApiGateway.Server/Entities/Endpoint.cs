using System.Collections.Generic;
using System.Text.Json.Serialization;
using Midos.SeedWork.Domain;

namespace Namei.ApiGateway.Server
{
  public class Endpoint: IEntity
  {
    public long Id { get; set; }

    public string Url { get; set; }

    public string Name { get; set; }

    [JsonIgnore]
    public List<Route> Routes { get; set; }

    public Endpoint() {}

    public static Endpoint From(
      string url,
      string name
    ) => new() {
      Url = url,
      Name = name
    };
  }
}
