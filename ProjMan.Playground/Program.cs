using ProjMan.Playground;

Project project = Project.CreateDraft("Проект");
User user1 = User.Create("Паша");
User user2 = User.Create("Леша");

project.SetAdministrator(user1.Id);
project.SetResponsible(user2.Id);

Console.WriteLine(project.ProjectAdministratorId);
Console.WriteLine(project.ProjectResponsibleId);
