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
    /// Пустое значение очищает поле, если текущая стадия допускает его отсутствие.
    /// </summary>
    public void UpdateCustomer(string? customer)
    {
        string? normalizedCustomer = NormalizeOptionalText(customer);
        if (Status == ProjectStatus.Tender && normalizedCustomer is null)
        {
            throw new InvalidOperationException("На стадии тендера заказчик обязателен");
        }

        if (Customer == normalizedCustomer) return;
        Customer = normalizedCustomer;
        MarkAsUpdated();
    }

    /// <summary>
    /// Изменяет расположение объекта.
    /// Пустое значение очищает поле, если текущая стадия допускает его отсутствие.
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
    /// Пустое значение очищает поле, если текущая стадия допускает его отсутствие.
    /// </summary>
    public void UpdateTenderNumber(string? tenderNumber)
    {
        string? normalizedTenderNumber = NormalizeOptionalText(tenderNumber);
        if (Status == ProjectStatus.Tender && normalizedTenderNumber is null)
        {
            throw new InvalidOperationException("На стадии тендера номер тендера обязателен");
        }

        if (TenderNumber == normalizedTenderNumber) return;
        TenderNumber = normalizedTenderNumber;
        MarkAsUpdated();
    }

    /// <summary>
    /// Назначает или изменяет ответственного за проект.
    /// Значение <see langword="null"/> снимает назначение, если текущая стадия допускает его отсутствие.
    /// </summary>
    public void SetResponsible(Guid? responsibleId)
    {
        if (responsibleId == Guid.Empty)
        {
            throw new ArgumentException(
                "Идентификатор ответственного не может быть пустым.",
                nameof(responsibleId)
            );
        }

        if (Status == ProjectStatus.Tender && responsibleId is null)
        {
            throw new InvalidOperationException("На стадии тендера ответственный обязателен");
        }

        if (responsibleId == ProjectResponsibleId) return;

        ProjectResponsibleId = responsibleId;

        MarkAsUpdated();
    }

    /// <summary>
    /// Назначает или изменяет администратора проекта.
    /// Значение <see langword="null"/> снимает назначение, если текущая стадия допускает его отсутствие.
    /// </summary>
    public void SetAdministrator(Guid? administratorId)
    {
        if (administratorId == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор администратора не может быть пустым.",
                nameof(administratorId));
        }

        if (Status == ProjectStatus.Tender && administratorId is null)
        {
            throw new InvalidOperationException("На стадии тендера администратор обязателен");
        }

        if (administratorId == ProjectAdministratorId) return;

        ProjectAdministratorId = administratorId;

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

    /// <summary>
    /// Создаёт проект в статусе тендера с обязательными сведениями
    /// и назначенными участниками.
    /// </summary>
    public static Project CreateTender(
        string name,
        string customer,
        string tenderNumber,
        Guid administratorId,
        Guid responsibleId)
    {
        string? normalizedCustomer = NormalizeOptionalText(customer);

        if (normalizedCustomer is null)
        {
            throw new ArgumentException(
                "Заказчик не может быть пустым.",
                nameof(customer));
        }

        string? normalizedTenderNumber = NormalizeOptionalText(tenderNumber);

        if (normalizedTenderNumber is null)
        {
            throw new ArgumentException(
                "Номер тендера не может быть пустым.",
                nameof(tenderNumber));
        }

        if (administratorId == Guid.Empty)
        {
            throw new ArgumentException(
                "Администратор проекта не может быть пустым.",
                nameof(administratorId));
        }

        if (responsibleId == Guid.Empty)
        {
            throw new ArgumentException(
                "Ответственный проекта не может быть пустым.",
                nameof(responsibleId));
        }

        Project project = new Project(name);
        project.Customer = normalizedCustomer;
        project.TenderNumber = normalizedTenderNumber;
        project.ProjectAdministratorId = administratorId;
        project.ProjectResponsibleId = responsibleId;
        project.Status = ProjectStatus.Tender;

        return project;
    }
    
    /// <summary>
    /// Изменяет реквизиты договора.
    /// Пустые строки и null очищают соответствующие поля.
    /// Дата изменения обновляется только при изменении реквизитов.
    /// </summary>
    public void UpdateContract(
        string? number,
        DateOnly? date,
        string? terms)
    {
        string? normalizedNumber = NormalizeOptionalText(number);
        string? normalizedTerms = NormalizeOptionalText(terms);

        if (ContractNumber == normalizedNumber &&
            ContractDate == date &&
            ContractTerms == normalizedTerms)
        {
            return;
        }

        ContractNumber = normalizedNumber;
        ContractDate = date;
        ContractTerms = normalizedTerms;

        MarkAsUpdated();
    }
}