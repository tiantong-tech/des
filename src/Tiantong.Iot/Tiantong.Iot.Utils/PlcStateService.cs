using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tiantong.Iot.Utils
{
  public class PlcStateService
  {
    private HttpClient _client;

    private string _plc;

    public PlcStateService(IHttpClientFactory factory)
    {
      _client = factory.CreateClient();
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    public PlcStateService(IHttpClientFactory factory, string uri, string plc)
    {
      _client = factory.CreateClient();
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
      Configure(uri, plc);
    }

    public void Configure(string uri, string plc)
    {
      _plc = plc;
      _client.BaseAddress = new Uri(uri);
    }

    public async Task SetAsync(string state, string value)
    {
      var text = JsonSerializer.Serialize(new {
        plc = _plc,
        state = state,
        value = value
      });

      var content = new StringContent(text, Encoding.UTF8, MediaTypeNames.Application.Json);
      var response = await _client.PostAsync("/plc-states/set", content);

      if (response.StatusCode != HttpStatusCode.Created) {
        var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        throw KnownException.Error(
          dom.RootElement.GetProperty("message").GetString(),
          (int) response.StatusCode
        );
      }
    }

    public async Task<string> GetAsync(string state)
    {
      var text = JsonSerializer.Serialize(new {
        plc = _plc,
        state = state
      });

      var content = new StringContent(text, Encoding.UTF8, MediaTypeNames.Application.Json);
      var response = await _client.PostAsync("/plc-states/get", content);
      var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

      if (response.StatusCode == HttpStatusCode.OK) {
        return dom.RootElement.GetProperty("value").GetString();
      } else {
        throw KnownException.Error(
          dom.RootElement.GetProperty("message").GetString(),
          (int) response.StatusCode
        );
      }
    }

    public async Task<string> CollectAsync(string state)
    {
      var text = JsonSerializer.Serialize(new {
        plc = _plc,
        state = state
      });

      var content = new StringContent(text, Encoding.UTF8, MediaTypeNames.Application.Json);
      var response = await _client.PostAsync("/plc-states/collect", content);
      var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

      if (response.StatusCode == HttpStatusCode.OK) {
        return dom.RootElement.GetProperty("value").GetString();
      } else {
        throw KnownException.Error(
          dom.RootElement.GetProperty("message").GetString(),
          (int) response.StatusCode
        );
      }
    }

    public async Task<Dictionary<string, string>> GetValuesAsync()
    {
      var text = JsonSerializer.Serialize(new {
        plc = _plc
      });

      var content = new StringContent(text, Encoding.UTF8, MediaTypeNames.Application.Json);
      var response = await _client.PostAsync("/plc-states/values", content);
      var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

      if (response.StatusCode == HttpStatusCode.OK) {
        var data = dom.RootElement.GetProperty("data");
        var result = new Dictionary<string, string>();

        foreach (var item in data.EnumerateObject()) {
          result.Add(item.Name, item.Value.GetString());
        }

        return result;
      } else {
        throw KnownException.Error(
          dom.RootElement.GetProperty("message").GetString(),
          (int) response.StatusCode
        );
      }
    }

    public void Set(string state, string value)
      => SetAsync(state, value).GetAwaiter().GetResult();

    public string Get(string state)
      => GetAsync(state).GetAwaiter().GetResult();

    public string Collect(string state)
      => CollectAsync(state).GetAwaiter().GetResult();

    public Dictionary<string, string> GetValues()
      => GetValuesAsync().GetAwaiter().GetResult();
  }
}
