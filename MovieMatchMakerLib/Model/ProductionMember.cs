namespace MovieMatchMakerLib.Model
{
    public class ProductionMember
    {
        public enum Type
        {
            Cast,
            Crew
        }

        public int Id { get; set; }
        public Name Name { get; set; }
        public string Job { get; set; }
        public Type MemberType { get; set; }
        public int ApiId { get; set; }

        //public ProductionMember(Name name, string job, Type memberType)
        //{
        //    Name = name;
        //    Job = job;
        //    MemberType = memberType;
        //}

    }
}
