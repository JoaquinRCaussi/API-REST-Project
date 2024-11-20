using System.Diagnostics.CodeAnalysis;
using BusinessLogic.DataAccessInterfaces;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Moq;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class MemberSettingLogicTest
{
    private Mock<IMemberSettingRepository>? _memberSettingRepositoryMock;
    private IMemberSettingLogic? _memberSettingLogic;

    [TestInitialize]
    public void Initialize()
    {
        _memberSettingRepositoryMock = new Mock<IMemberSettingRepository>();
        _memberSettingLogic = new MemberSettingLogic(_memberSettingRepositoryMock.Object);
    }

    [TestMethod]
    public void CreateMemberSettingTest()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var memberSetting = new MemberSetting
        {
            HomeId = homeId,
            UserId = userId
        };

        _memberSettingRepositoryMock?.Setup(x => x.CreateMemberSetting(homeId, userId)).Returns(memberSetting);

        var result = _memberSettingLogic?.CreateMemberSetting(homeId, userId);

        result.Should().BeEquivalentTo(memberSetting);
    }

    [TestMethod]
    public void GetMemberSettingsTest()
    {
        var memberSettings = new List<MemberSetting>
        {
            new MemberSetting { HomeId = Guid.NewGuid(), UserId = Guid.NewGuid() }
        };

        _memberSettingRepositoryMock?.Setup(x => x.GetMemberSettings()).Returns(memberSettings);

        var result = _memberSettingLogic?.GetMemberSettings();

        result.Should().BeEquivalentTo(memberSettings);
    }

    [TestMethod]
    public void GetMemberSettingTest()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var memberSetting = new MemberSetting
        {
            HomeId = homeId,
            UserId = userId
        };

        _memberSettingRepositoryMock?.Setup(x => x.GetMemberSetting(homeId, userId)).Returns(memberSetting);

        var result = _memberSettingLogic?.GetMemberSetting(homeId, userId);

        result.Should().BeEquivalentTo(memberSetting);
    }

    [TestMethod]
    public void GetMemberSettingNotFoundTest()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _memberSettingRepositoryMock?.Setup(x => x.GetMemberSetting(homeId, userId)).Returns((MemberSetting?)null);

        Action act = () => _memberSettingLogic?.GetMemberSetting(homeId, userId);

        act.Should().Throw<EmptyException>().WithMessage("No member setting found for this user at this home.");
    }

    [TestMethod]
    public void UpdateMemberSettingTest()
    {
        var memberSetting = new MemberSetting
        {
            HomeId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Permissions = []
        };

        _memberSettingRepositoryMock?.Setup(x => x.UpdateMemberSetting(memberSetting)).Returns(memberSetting);

        var result = _memberSettingLogic?.UpdateMemberSetting(memberSetting);

        result.Should().BeEquivalentTo(memberSetting);
    }

    [TestMethod]
    public void DeleteMemberSettingTest()
    {
        var memberSettingId = Guid.NewGuid();
        var memberSetting = new MemberSetting
        {
            Id = memberSettingId,
            HomeId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        _memberSettingRepositoryMock?.Setup(x => x.DeleteMemberSetting(memberSettingId)).Returns(memberSetting);

        var result = _memberSettingLogic?.DeleteMemberSetting(memberSettingId);

        result.Should().BeEquivalentTo(memberSetting);
    }

    [TestMethod]
    public void AddPermissionTest()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var permission = "CanAddMembers";
        var memberSetting = new MemberSetting
        {
            HomeId = homeId,
            UserId = userId,
            Permissions = [new Permission { Value = permission }]
        };

        _memberSettingRepositoryMock?.Setup(x => x.AddPermission(homeId, userId, permission)).Returns(memberSetting);

        var result = _memberSettingLogic?.AddPermission(homeId, userId, permission);

        result.Should().BeEquivalentTo(memberSetting);
    }

    [TestMethod]
    public void RemovePermissionTest()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var permission = "CanAddMembers";
        var memberSetting = new MemberSetting
        {
            HomeId = homeId,
            UserId = userId,
            Permissions = []
        };

        _memberSettingRepositoryMock?.Setup(x => x.RemovePermission(homeId, userId, permission)).Returns(memberSetting);

        var result = _memberSettingLogic?.RemovePermission(homeId, userId, permission);

        result.Should().BeEquivalentTo(memberSetting);
    }

    [TestMethod]
    public void HasPermissionTest()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var permission = "CanAddMembers";

        _memberSettingRepositoryMock?.Setup(x => x.HasPermission(homeId, userId, permission)).Returns(true);

        var result = _memberSettingLogic?.HasPermission(homeId, userId, permission);

        result.Should().BeTrue();
    }
}
