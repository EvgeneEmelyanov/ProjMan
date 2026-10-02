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
    public void UpdateContract_WhenAllValuesAreNull_ClearsContract(
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
}