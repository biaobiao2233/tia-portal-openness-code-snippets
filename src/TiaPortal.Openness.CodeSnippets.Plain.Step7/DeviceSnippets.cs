// © Siemens 2025 - 2026
// Licensed under: "Royalty-free Software provided by Siemens on sharing platforms for developers/users of Siemens products". See LICENSE.md.

using NUnit.Framework;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using TiaPortal.Openness.CodeSnippets.Plain.Setup;

namespace TiaPortal.Openness.CodeSnippets.Plain.Step7;

[TestFixture("Step7.zap21")]
public class DeviceSnippets(string tiaArchiveName) : BaseClass(tiaArchiveName)
{
    [Test]
    public void EnumerateAllDevicesIncludesRootDevices()
    {
        var devices = EnumerateAllDevices(Project);

        Assert.That(devices, Is.SupersetOf(Project.Devices));
        Assert.That(devices.Select(device => device.Name), Has.All.Not.Empty);
    }

    /// <summary>
    /// Plain Openness equivalent of <c>Project.AllDevices()</c> from the Openness Extensions package.
    /// Devices can live in the project root, in nested device groups, or in the ungrouped devices group.
    /// </summary>
    public static IReadOnlyList<Device> EnumerateAllDevices(Project project)
    {
        var devices = new List<Device>();

        CollectDevices(project.Devices, devices);
        foreach (var group in project.DeviceGroups)
        {
            CollectGroup(group, devices);
        }

        var ungrouped = project.UngroupedDevicesGroup;
        if (ungrouped != null)
        {
            CollectDevices(ungrouped.Devices, devices);
        }

        return devices;
    }

    private static void CollectGroup(DeviceUserGroup group, List<Device> devices)
    {
        CollectDevices(group.Devices, devices);
        foreach (var child in group.Groups)
        {
            CollectGroup(child, devices);
        }
    }

    private static void CollectDevices(IEnumerable<Device> devices, List<Device> result)
    {
        foreach (var device in devices)
        {
            result.Add(device);
        }
    }
}
