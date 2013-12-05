using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;

namespace XBMCRPC
{
	public partial class Client:IDisposable
	{
		private readonly ConnectionSettings _settings;
		private uint JsonRpcId = 0;

				public Methods.Addons Addons { get; private set; }
		public Methods.Application Application { get; private set; }
		public Methods.AudioLibrary AudioLibrary { get; private set; }
		public Methods.Files Files { get; private set; }
		public Methods.GUI GUI { get; private set; }
		public Methods.Input Input { get; private set; }
		public Methods.JSONRPC JSONRPC { get; private set; }
		public Methods.Player Player { get; private set; }
		public Methods.Playlist Playlist { get; private set; }
		public Methods.PVR PVR { get; private set; }
		public Methods.System System { get; private set; }
		public Methods.VideoLibrary VideoLibrary { get; private set; }
		public Methods.XBMC XBMC { get; private set; }

		public Client(ConnectionSettings settings)
		{
			Serializer = new JsonSerializer();
			Serializer.Converters.Add(new StringEnumConverter());
					   Serializer.Converters.Add(new BaseConverter(MultipleInheritanceKeyBase));
		   Serializer.Converters.Add(new AllConverter(MultipleInheritanceKeyAll));
		   Serializer.Converters.Add(new FileConverter(MultipleInheritanceKeyFile));
			_settings = settings;
					   Addons = new Methods.Addons(this);
		   Application = new Methods.Application(this);
		   AudioLibrary = new Methods.AudioLibrary(this);
		   Files = new Methods.Files(this);
		   GUI = new Methods.GUI(this);
		   Input = new Methods.Input(this);
		   JSONRPC = new Methods.JSONRPC(this);
		   Player = new Methods.Player(this);
		   Playlist = new Methods.Playlist(this);
		   PVR = new Methods.PVR(this);
		   System = new Methods.System(this);
		   VideoLibrary = new Methods.VideoLibrary(this);
		   XBMC = new Methods.XBMC(this);
		}

		internal JsonSerializer Serializer { get; private set; }

		 internal T GetData<T>(string method, object args)
		{
			var request = WebRequest.Create(_settings.JsonInterfaceAddress);
			request.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);
			request.ContentType = "application/json";
			request.Method = "POST";
			var postStream =  request.GetRequestStream();

			var requestId = JsonRpcId++;
			var jsonRequest = BuildJsonPost(method, args, requestId);
			byte[] postData = Encoding.UTF8.GetBytes(jsonRequest);
			postStream.Write(postData, 0, postData.Length);
			postStream.Dispose();

			var response =  request.GetResponse();
			var responseStream = response.GetResponseStream();
			string responseData=null;
			if (responseStream != null)
			{
				var streamReader = new StreamReader(responseStream);
				responseData = streamReader.ReadToEnd();
				responseStream.Dispose();
				streamReader.Dispose();
			}

			response.Close();

			JObject query = JObject.Parse(responseData);
			var error = query["error"];
			if (error!=null)
			{
				throw new Exception(error.ToString());
			}
			var result = query["result"].ToObject<T>(Serializer);
			return result;
		}

		private static string BuildJsonPost(string method, object args, uint id)
		{
			var jsonPost = new JObject {new JProperty("jsonrpc", "2.0"), new JProperty("method", method)};
			if (args != null)
			{
				jsonPost.Add(new JProperty("params", args));
			}
			jsonPost.Add(new JProperty("id", id));

			return jsonPost.ToString();
		}

		
		private Socket _clientSocket;

		//public  void StartNotificationListener()
		//{
		//	_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
		//	var socketState = new NotificationListenerSocketState();
		//	 Task.Factory.From(_clientSocket.BeginConnect, _clientSocket.EndConnect, _settings.Host, _settings.TcpPort, socketState);
		//	_clientSocket.BeginReceive(socketState.Buffer, 0, NotificationListenerSocketState.BufferSize, SocketFlags.None,
		//							   ReceivedData, socketState);
		//}

		//private void ReceivedData(IResult Result)
		//{
		//	var socketState = Result.State as NotificationListenerSocketState;

		//	if (socketState != null)
		//	{
		//		var receivedDataLength = _clientSocket.EndReceive(Result);
		//		var receivedDataJson = Encoding.UTF8.GetString(socketState.Buffer, 0, receivedDataLength);

		//		socketState.Builder.Append(receivedDataJson);

		//		JObject jObject;
		//		if (TryParseObject(socketState.Builder.ToString(), out jObject))
		//		{
		//			ParseNotification(jObject);

		//			var freshSocketState = new NotificationListenerSocketState();

		//			_clientSocket.BeginReceive(freshSocketState.Buffer, 0, NotificationListenerSocketState.BufferSize,
		//									   SocketFlags.None,
		//									   ReceivedData, freshSocketState);
		//		}
		//		else
		//		{
		//			// Begin listening for remainder of announcement using same socket state
		//			_clientSocket.BeginReceive(socketState.Buffer, 0, NotificationListenerSocketState.BufferSize,
		//									   SocketFlags.None,
		//									   ReceivedData, socketState);
		//		}
		//	}
		//}

		private static bool TryParseObject(string announcementJson, out JObject jObject)
		{
			jObject = null;
			try
			{
				jObject = JObject.Parse(announcementJson);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}


		private void ParseNotification(JObject jObject)
		{
			if (jObject["method"] != null)
			{
				string _method;
				_method = jObject["method"].ToString();
				switch (_method)
				{
							case "Application.OnVolumeChanged":
			Application.RaiseOnVolumeChanged(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Application.OnVolumeChangeddataType>(Serializer)
);
			break;
		case "AudioLibrary.OnCleanFinished":
			AudioLibrary.RaiseOnCleanFinished(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "AudioLibrary.OnCleanStarted":
			AudioLibrary.RaiseOnCleanStarted(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "AudioLibrary.OnRemove":
			AudioLibrary.RaiseOnRemove(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.AudioLibrary.OnRemovedataType>(Serializer)
);
			break;
		case "AudioLibrary.OnScanFinished":
			AudioLibrary.RaiseOnScanFinished(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "AudioLibrary.OnScanStarted":
			AudioLibrary.RaiseOnScanStarted(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "AudioLibrary.OnUpdate":
			AudioLibrary.RaiseOnUpdate(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.AudioLibrary.OnUpdatedataType>(Serializer)
);
			break;
		case "Input.OnInputFinished":
			Input.RaiseOnInputFinished(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "Input.OnInputRequested":
			Input.RaiseOnInputRequested(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Input.OnInputRequesteddataType>(Serializer)
);
			break;
		case "Player.OnPause":
			Player.RaiseOnPause(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<XBMCRPC.Player.Notifications.Data>(Serializer)
);
			break;
		case "Player.OnPlay":
			Player.RaiseOnPlay(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<XBMCRPC.Player.Notifications.Data>(Serializer)
);
			break;
		case "Player.OnPropertyChanged":
			Player.RaiseOnPropertyChanged(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Player.OnPropertyChangeddataType>(Serializer)
);
			break;
		case "Player.OnSeek":
			Player.RaiseOnSeek(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Player.OnSeekdataType>(Serializer)
);
			break;
		case "Player.OnSpeedChanged":
			Player.RaiseOnSpeedChanged(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<XBMCRPC.Player.Notifications.Data>(Serializer)
);
			break;
		case "Player.OnStop":
			Player.RaiseOnStop(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Player.OnStopdataType>(Serializer)
);
			break;
		case "Playlist.OnAdd":
			Playlist.RaiseOnAdd(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Playlist.OnAdddataType>(Serializer)
);
			break;
		case "Playlist.OnClear":
			Playlist.RaiseOnClear(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Playlist.OnCleardataType>(Serializer)
);
			break;
		case "Playlist.OnRemove":
			Playlist.RaiseOnRemove(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.Playlist.OnRemovedataType>(Serializer)
);
			break;
		case "System.OnLowBattery":
			System.RaiseOnLowBattery(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "System.OnQuit":
			System.RaiseOnQuit(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "System.OnRestart":
			System.RaiseOnRestart(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "System.OnSleep":
			System.RaiseOnSleep(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "System.OnWake":
			System.RaiseOnWake(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "VideoLibrary.OnCleanFinished":
			VideoLibrary.RaiseOnCleanFinished(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "VideoLibrary.OnCleanStarted":
			VideoLibrary.RaiseOnCleanStarted(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "VideoLibrary.OnRemove":
			VideoLibrary.RaiseOnRemove(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.VideoLibrary.OnRemovedataType>(Serializer)
);
			break;
		case "VideoLibrary.OnScanFinished":
			VideoLibrary.RaiseOnScanFinished(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "VideoLibrary.OnScanStarted":
			VideoLibrary.RaiseOnScanStarted(
jObject["params"]["sender"].ToObject<string>(Serializer)
);
			break;
		case "VideoLibrary.OnUpdate":
			VideoLibrary.RaiseOnUpdate(
jObject["params"]["sender"].ToObject<string>(Serializer)
, jObject["params"]["data"].ToObject<Methods.VideoLibrary.OnUpdatedataType>(Serializer)
);
			break;

				}
			}
		}

		public void Dispose()
		{
			_clientSocket.Disconnect(false);
			_clientSocket.Dispose();
		}

	}
}