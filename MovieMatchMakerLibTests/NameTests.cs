using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib;

namespace MovieMatchMakerLibTests
{
    public class NameTests
    {
        [Fact]
        public void Test_FirstName_Surname()
        {
            var fullName = "Nathan Miller";
            var name = new Name(fullName);
            Assert.Equal("Nathan", name.FirstName);
            Assert.Equal("Miller", name.Surname);
        }

        [Fact]
        public void Test_Surname_Comma_FirstName()
        {
            var fullName = "Miller, Nathan";
            var name = new Name(fullName);
            Assert.Equal("Nathan", name.FirstName);
            Assert.Equal("Miller", name.Surname);
        }

        [Fact]
        public void Test_OneName()
        {
            var fullName = "Nathan";
            var name = new Name(fullName);
            Assert.Equal("Nathan", name.FirstName);
            Assert.Equal("", name.Surname);
        }

        [Fact]
        public void Test_Two_Arg_Ctor_FirstName_Surname()
        {            
            var name = new Name("Nathan", "Miller");
            Assert.Equal("Nathan", name.FirstName);
            Assert.Equal("Miller", name.Surname);
        }
    }
}
