namespace linghub.Dto
{
    public class UserDto
    {
        public int IdUser { get; set; }

        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;
    }
}
