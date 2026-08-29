namespace ProjMan.Playground;

/// <summary>
/// Представляет инженерный проект, для которого ведутся задачи, расчёты и проектная документация.
/// </summary>
public class Project
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string? ProjectTitle { get; private set; }
    public string? Customer { get; private set; }
    public string? Location { get; private set; }
    public string? TenderNumber { get; private set; }

    public Guid? ProjectAdministratorId { get; private set; }
    public Guid? ProjectResponsibleId { get; private set; }

    public string? ContractNumber { get; private set; }
    public DateOnly? ContractDate { get; private set; }
    public string? ContractTerms { get; private set; }

    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public ProjectStatus Status { get; private set; }


    private Project(string name)
    {
        Id = Guid.NewGuid();
        Name = NormalizeName(name);
        Status = ProjectStatus.Draft;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    private void MarkAsUpdated()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Имя проекта не может быть пустым", nameof(name));
        }

        return name.Trim();
    }

    private static string? NormalizeOptionalText(string? text)
    {
        return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
    }

    public void Rename(string name)
    {
        string normalizedName = NormalizeName(name);
        if (Name == normalizedName) return;
        Name = normalizedName;
        MarkAsUpdated();
    }

    /// <summary>
    /// Изменяет официальное наименование проекта.
    /// Пустое значение очищает поле.
    /// </summary>
    public void UpdateProjectTitle(string? projectTitle)
    {
        string? normalizedProjectTitle = NormalizeOptionalText(projectTitle);
        if (ProjectTitle == normalizedProjectTitle) return;
        ProjectTitle = normalizedProjectTitle;
        MarkAsUpdated();
    }

    /// <summary>
    /// Изменяет заказчика проекта.
    /// Пустое значение очищает поле.
    /// </summary>
    public void UpdateCustomer(string? customer)
    {
        string? normalizedCustomer = NormalizeOptionalText(customer);
        if (Customer == normalizedCustomer) return;
        Customer = normalizedCustomer;
        MarkAsUpdated();
    }

    /// <summary>
    /// Изменяет расположение объекта.
    /// Пустое значение очищает поле.
    /// </summary>
    public void UpdateLocation(string? location)
    {
        string? normalizedLocation = NormalizeOptionalText(location);
        if (Location == normalizedLocation) return;
        Location = normalizedLocation;
        MarkAsUpdated();
    }

    /// <summary>
    /// Изменяет номер тендера проекта.
    /// Пустое значение очищает поле.
    /// </summary>
    public void UpdateTenderNumber(string? tenderNumber)
    {
        string? normalizedTenderNumber = NormalizeOptionalText(tenderNumber);
        if (TenderNumber == normalizedTenderNumber) return;
        TenderNumber = normalizedTenderNumber;
        MarkAsUpdated();
    }

    /// <summary>
    /// Назначает или изменяет ответственного за проект.
    /// Значение <see langword="null"/> снимает назначение.
    /// </summary>
    public void SetResponsible(Guid? projectResponsibleId)
    {
        if (projectResponsibleId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор ответственного не может быть пустым.", nameof(projectResponsibleId));
        }

        if (projectResponsibleId == ProjectResponsibleId) return;

        ProjectResponsibleId = projectResponsibleId;

        MarkAsUpdated();
    }


    /// <summary>
    /// Назначает или изменяет администратора проекта.
    /// Значение <see langword="null"/> снимает назначение.
    /// </summary>
    public void SetAdministrator(Guid? projectAdministratorId)
    {
        if (projectAdministratorId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор администратора не может быть пустым.", nameof(projectAdministratorId));
        }

        if (projectAdministratorId == ProjectAdministratorId) return;

        ProjectAdministratorId = projectAdministratorId;

        MarkAsUpdated();
    }

    /// <summary>
    /// Создаёт проект в статусе черновика.
    /// Для создания требуется только внутреннее название проекта.
    /// </summary>
    public static Project CreateDraft(string name)
    {
        return new Project(name);
    }
}