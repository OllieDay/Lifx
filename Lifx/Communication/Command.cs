namespace Lifx.Communication
{
	// Represents the type of request/response.
	internal enum Command : ushort
	{
		// All devices
		DeviceGetService = 2,
		DeviceStateService = 3,
		DeviceGetHostInfo = 12,
		DeviceStateHostInfo = 13,
		DeviceGetHostFirmware = 14,
		DeviceStateHostFirmware = 15,
		DeviceGetWifiInfo = 16,
		DeviceStateWifiInfo = 17,
		DeviceGetWifiFirmware = 18,
		DeviceStateWifiFirmware = 19,
		DeviceGetPower = 20,
		DeviceSetPower = 21,
		DeviceStatePower = 22,
		DeviceGetLabel = 23,
		DeviceSetLabel = 24,
		DeviceStateLabel = 25,
		DeviceGetVersion = 32,
		DeviceStateVersion = 33,
		DeviceGetInfo = 34,
		DeviceStateInfo = 35,
		DeviceAcknowledgement = 45,
		DeviceGetLocation = 48,
		DeviceStateLocation = 50,
		DeviceGetGroup = 51,
		DeviceStateGroup = 53,
		DeviceEchoRequest = 58,
		DeviceEchoResponse = 59,

		// Lights only
		LightGet = 101,
		LightSetColor = 102,
		LightState = 107,
		LightGetPower = 116,
		LightSetPower = 117,
		LightStatePower = 118
	}
}
