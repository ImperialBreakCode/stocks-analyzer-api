using API.Accounts.Application.Settings.GatewaySecretKeySender;
using API.Accounts.Application.Settings.Options;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace API.Accounts.Application.Settings.GatewaySettingsSender
{
    public class SocketGatewaySettingsSender : IGatewaySettingsSender
    {
        public async Task<bool> SendAuthTokenSettingsToGateway(AuthValues gatewayKey, string gatewaySocketHost)
        {
            string dataJson = JsonConvert.SerializeObject(gatewayKey);

            using (var webSocketClient = new ClientWebSocket())
            {
                Uri uri = new(gatewaySocketHost + "/someRoute");

                try
                {
                    await webSocketClient.ConnectAsync(uri, CancellationToken.None);
                }
                catch (WebSocketException)
                {
                    return false;
                }

                byte[] buffer = Encoding.UTF8.GetBytes(dataJson);
                await webSocketClient.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

                try
                {
                    buffer = new byte[1024];
                    var timeOut = new CancellationTokenSource(10000).Token;
                    var result = await webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), timeOut);

                    await Task.Delay(1000);
                    await webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);

                    return true;
                }
                catch (OperationCanceledException)
                {
                    return false;
                }
            }
        }
    }
}
