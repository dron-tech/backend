namespace Domain.Common.Enums;

public enum UserStatus
{
    Dancer = 1,
    Dj = 2,
    Host = 3,
    Model = 4,
    Musician = 5,
    Photographer = 6,
    Producer = 7,
    Promoter = 8,
    Singer = 9,
    Songwriter = 10,
    Venue = 11
}

public static class UserStatusMapper
{
    public static string MapToStr(UserStatus status)
    {
        return status switch
        {
            UserStatus.Dancer => "Dancer",
            UserStatus.Dj => "DJ",
            UserStatus.Host => "Host",
            UserStatus.Model => "Model",
            UserStatus.Musician => "Musician",
            UserStatus.Photographer => "Photographer",
            UserStatus.Producer => "Producer",
            UserStatus.Promoter => "Promoter",
            UserStatus.Singer => "Signer",
            UserStatus.Songwriter => "Songwriter",
            UserStatus.Venue => "Venue",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Out of enum range")
        };
    }
}
