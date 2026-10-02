namespace ProjMan.Playground.Tests;

public class ProjectTests
{
    [Test]
    public void Rename_WhenNormalizedNameIsUnchanged_DoesNotChangeUpdatedAt()
    {
        // Arrange: подготавливаем проект и запоминаем исходное состояние.
        var project = Project.CreateDraft("БКТП 630 кВА");
        var originalUpdatedAt = project.UpdatedAt;

        // Act: выполняем действие, которое проверяем.
        project.Rename("  БКТП 630 кВА  ");

        // Assert: проверяем ожидаемый результат.
        Assert.That(project.Name, Is.EqualTo("БКТП 630 кВА"));
        Assert.That(project.UpdatedAt, Is.EqualTo(originalUpdatedAt));
    }

    [Test]
    public void Rename_WhenNameIsValid_UpdatesNameAndTrimsWhitespace()
    {
        // Arrange: подготавливаем проект с исходным названием
        var project = Project.CreateDraft("БКТП 630 кВА");

        // Act: переименовываем проект в новое название с пробелами
        project.Rename("  2БКТП 2500 кВА  ");

        // Assert: проверка ожидаемого результата
        Assert.That(project.Name, Is.EqualTo("2БКТП 2500 кВА"));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase(null)]
    public void Rename_WhenNameIsNullOrWhitespace_ThrowsArgumentException(
        string? invalidName)
    {
        // Arrange
        var project = Project.CreateDraft("БКТП 630 кВА");
        var originalUpdatedAt = project.UpdatedAt;

        // Act, Assert
        Assert.Throws<ArgumentException>(() => project.Rename(invalidName));

        // Assert
        Assert.That(project.Name, Is.EqualTo("БКТП 630 кВА"));
        Assert.That(project.UpdatedAt, Is.EqualTo(originalUpdatedAt));
    }

    [Test]
    public void CreateDraft_WhenNameIsValid_CreatesDraftWithTrimmedName()
    {
        // Act
        var project = Project.CreateDraft("  БКТП 630 кВА  ");

        // Assert
        Assert.That(project.Name, Is.EqualTo("БКТП 630 кВА"));
        Assert.That(project.Status, Is.EqualTo(ProjectStatus.Draft));
        Assert.That(project.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(project.UpdatedAt, Is.EqualTo(project.CreatedAt));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("\t")]
    public void CreateDraft_WhenNameIsNullOrWhitespace_ThrowsArgumentException(
        string? invalidName)
    {
        // Act, assert
        Assert.Throws<ArgumentException>(() => Project.CreateDraft(invalidName));
    }

    [Test]
    public void UpdateContract_WhenValuesAreValid_SetsNormalizedValues()
    {
        // Arrange
        var project = Project.CreateDraft("БКТП 630 кВА");

        // Act
        project.UpdateContract("  Д-15  ", new DateOnly(2026, 10, 2), "  Аванс 30%   ");

        // Assert 
        Assert.That(project.ContractNumber, Is.EqualTo("Д-15"));
        Assert.That(project.ContractDate, Is.EqualTo(new DateOnly(2026, 10, 2)));
        Assert.That(project.ContractTerms, Is.EqualTo("Аванс 30%"));
    }
    
    [Test]
    public void UpdateContract_WhenNormalizedValuesAreUnchanged_DoesNotChangeUpdatedAt()
    {
        // Arrange
        var project = Project.CreateDraft("БКТП 630 кВА");
        project.UpdateContract("  Д-15  ", new DateOnly(2026, 10, 2), "  Аванс 30%   ");
        var originalUpdatedAt = project.UpdatedAt;
       
        // Act
        project.UpdateContract("  Д-15  ", new DateOnly(2026, 10, 2), "  Аванс 30%   ");

        // Assert
        Assert.That(project.UpdatedAt, Is.EqualTo(originalUpdatedAt));
    }
    
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("\t")]
    public void UpdateContract_WhenValuesAreEmpty_ClearsContract(
        string? emptyTest)
    {
        // Arange
        var project = Project.CreateDraft("БКТП 630 кВА");
        project.UpdateContract("  Д-15  ", new DateOnly(2026, 10, 2), "  Аванс 30%   ");
        
        // Act
        project.UpdateContract(emptyTest, null, emptyTest);
        
        // Assert
        Assert.That(project.ContractNumber, Is.Null);
        Assert.That(project.ContractDate, Is.Null);
        Assert.That(project.ContractTerms, Is.Null);
            
    }
    
    [Test]
    public void MoveToTender_WhenRequiredDataIsFilled_PreservesProjectIdentity()
    {
        // Arrange
        var project = Project.CreateDraft("БКТП 630 кВА");

        project.UpdateCustomer("Заказчик");
        project.UpdateTenderNumber("Т-15");
        project.SetAdministrator(Guid.NewGuid());
        project.SetResponsible(Guid.NewGuid());

        var originalId = project.Id;
        var originalCreatedAt = project.CreatedAt;

        // Act
        project.MoveToTender();

        // Assert
        Assert.That(project.Status, Is.EqualTo(ProjectStatus.Tender));
        Assert.That(project.Id, Is.EqualTo(originalId));
        Assert.That(project.CreatedAt, Is.EqualTo(originalCreatedAt));
    }

    [TestCase(null, "Т-15", true, true)]
    [TestCase("Заказчик", null, true, true)]
    [TestCase("Заказчик", "Т-15", false, true)]
    [TestCase("Заказчик", "Т-15", true, false)]
    public void MoveToTender_WhenRequiredDataIsMissing_ThrowsAndPreservesState(
        string? customer,
        string? tenderNumber,
        bool hasAdministrator,
        bool hasResponsible)
    {
        // Arrange
        var project = Project.CreateDraft("БКТП 630 кВА");

        Guid? administratorId = hasAdministrator ? Guid.NewGuid() : null;
        Guid? responsibleId = hasResponsible ? Guid.NewGuid() : null;

        project.UpdateCustomer(customer);
        project.UpdateTenderNumber(tenderNumber);
        project.SetAdministrator(administratorId);
        project.SetResponsible(responsibleId);

        var originalUpdatedAt = project.UpdatedAt;

        // Act, Assert
        Assert.Throws<InvalidOperationException>(() => project.MoveToTender());

        Assert.That(project.Status, Is.EqualTo(ProjectStatus.Draft));
        Assert.That(project.UpdatedAt, Is.EqualTo(originalUpdatedAt));
    }
    
    [Test]
    public void MoveToTender_WhenAlreadyTender_ThrowsAndPreservesState()
    {
        // Arrange
        var project = Project.CreateTender(
            "БКТП 630 кВА",
            "Заказчик",
            "Т-15",
            Guid.NewGuid(),
            Guid.NewGuid());
        
        var originalUpdateAt = project.UpdatedAt;
        
        // Act, assert
        Assert.Throws<InvalidOperationException>(() => project.MoveToTender());
        
        Assert.That(project.Status, Is.EqualTo(ProjectStatus.Tender));
        Assert.That(project.UpdatedAt, Is.EqualTo(originalUpdateAt));
    }
}