namespace ProjMan.Playground;

/// <summary>
/// Представляет пользователя системы, который может работать с проектом.
/// </summary>
public class User
{
    public Guid Id { get; }
    public string DisplayName { get; private set; }

    private User(string displayName)
    {
        Id = Guid.NewGuid();
        DisplayName = NormalizeDisplayName(displayName);
    }

    private static string NormalizeDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Имя пользователя не может быть пустым", nameof(displayName));
        }

        return displayName.Trim();
    }

    /// <summary>
    /// Создаёт аккаунт пользователя.
    /// </summary>
    public static User Create(string displayName)
    {
        return new User(displayName);
    }
}