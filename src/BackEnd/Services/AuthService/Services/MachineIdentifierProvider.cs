using AuthService.Services.Interfaces;
using System.Net.NetworkInformation;

namespace AuthService.Services;

public class MachineIdentifierProvider : IMachineIdentifierProvider
{
    public string GetIdentifier()
    {
        var id = GetMacAddress();
        return !string.IsNullOrWhiteSpace(id) ? id : GetMachineGuid();
    }

    private string GetMacAddress()
    {
        return NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();
    }

    private string GetMachineGuid()
    {
        var name = Environment.MachineName;
        return Guid.TryParse(name, out var guid) ? guid.ToString() : Guid.NewGuid().ToString();
    }
}