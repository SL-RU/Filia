using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filia.Shared;
using SQLite;

namespace Filia.Server
{


    public class DbUserData : IUserData
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string Nickname { get; set; }

        public byte[] Password { get; set; }

        [MaxLength(64)]
        public string Realname { get; set; }

        [MaxLength(512)]
        public string About { get; set; }

        [MaxLength(64)]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        public bool UploadImages { get; set; }
    }

    public class DbPhraseData : IPhraseData
    {
        public int Id { get; set; }
        public string Original { get; set; }
        public string Translated { get; set; }
        public string AuthorOfTranslation { get; set; }
        public string Tags { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorOfRevision { get; set; }
        [Ignore]
        public PhraseChangeHistory ChangeHistory { get; set; }
    }

}
