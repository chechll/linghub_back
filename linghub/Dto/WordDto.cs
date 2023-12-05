namespace linghub.Dto
{
    public class WordDto
    {
        public int IdWord { get; set; }

        public string Enword { get; set; } = null!;

        public string Ruword { get; set; } = null!;

        public string Ensent { get; set; } = null!;

        public string Rusent { get; set; } = null!;

        public string Ans1 { get; set; } = null!;

        public string Ans2 { get; set; } = null!;

        public string Ans3 { get; set; } = null!;
    }
}
