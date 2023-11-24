using API.Accounts.Application.Settings.GatewayAuthSettingsSender;
using API.Accounts.Application.Settings.Options;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace API.Accounts.Application.Settings.GatewaySettingsSender
{
    internal class SocketGatewaySettingsSender : ISocketGatewaySettingsSender
    {
        private ClientWebSocket _clientWebSocket;

        public async Task<bool> SendAuthTokenSettingsToGateway(AuthValues gatewayAuthValues, string gatewaySocketHost)
        {
            string dataJson = JsonConvert.SerializeObject(gatewayAuthValues);

            _clientWebSocket = new();

            bool connectionSuccessfull = await ConnectAsync(gatewaySocketHost);
            if (connectionSuccessfull)
            {
                await SendAuthDataToGateway(dataJson);
                await Complete();
            }

            _clientWebSocket.Dispose();

            return connectionSuccessfull;
        }

        private async Task Complete()
        {
            byte[] buffer = new byte[1024];
            var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            }
        }

        private async Task SendAuthDataToGateway(string jsonData)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(jsonData);
            await _clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<bool> ConnectAsync(string gatewaySocketHost)
        {
            Uri uri = new(gatewaySocketHost + "/someRoute");

            try
            {
                await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);
                return true;
            }
            catch (WebSocketException)
            {
                return false;
            }
        }
    }
}
