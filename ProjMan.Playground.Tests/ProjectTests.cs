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
}