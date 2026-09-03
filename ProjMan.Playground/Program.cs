using ProjMan.Playground;

User administrator = User.Create("Паша");
User responsible = User.Create("Лёша");

Project tender = Project.CreateTender(
    name: "БКТП 630 кВА",
    customer: "Военстрой",
    tenderNumber: "Т-2026-001",
    administratorId: administrator.Id,
    responsibleId: responsible.Id);

Console.WriteLine($"Имя: {tender.Name}");
Console.WriteLine($"Статус: {tender.Status}");
Console.WriteLine($"Заказчик: {tender.Customer}");
Console.WriteLine($"Администратор: {tender.ProjectAdministratorId}");
Console.WriteLine($"Ответственный: {tender.ProjectResponsibleId}");
Console.WriteLine(tender.CreatedAt == tender.UpdatedAt);























