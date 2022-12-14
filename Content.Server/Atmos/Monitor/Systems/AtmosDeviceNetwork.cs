using Content.Server.DeviceNetwork;
using Content.Server.DeviceNetwork.Systems;
using Content.Shared.Atmos.Monitor.Components;

namespace Content.Server.Atmos.Monitor.Systems;

/// <summary>
///     Generic device network commands useful for atmos devices,
///     as well as some helper commands.
/// </summary>
public sealed class AtmosDeviceNetworkSystem : EntitySystem
{
    /// <summary>
    ///     Register a device's address on this device.
    /// </summary>
    public const string RegisterDevice = "atmos_register_device";

    /// <summary>
    ///     Synchronize the data this device has with the sender.
    /// </summary>
    public const string SyncData = "atmos_sync_data";

    [Dependency] private readonly DeviceNetworkSystem _deviceNet = default!;

    public void Register(EntityUid uid, string? address)
    {
        var registerPayload = new NetworkPayload
        {
            [DeviceNetworkConstants.Command] = RegisterDevice
        };

        _deviceNet.QueuePacket(uid, address, registerPayload);
    }

    public void Sync(EntityUid uid, string? address)
    {
        var syncPayload = new NetworkPayload
        {
            [DeviceNetworkConstants.Command] = SyncData
        };

        _deviceNet.QueuePacket(uid, address, syncPayload);
    }

    public void SetDeviceState(EntityUid uid, string address, IAtmosDeviceData data)
    {
        var payload = new NetworkPayload()
        {
            [DeviceNetworkConstants.Command] = DeviceNetworkConstants.CmdSetState,
            [DeviceNetworkConstants.CmdSetState] = data
        };

        _deviceNet.QueuePacket(uid, address, payload);
    }
}
