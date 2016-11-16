using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        [Ignore]
        public bool Online
        {
            get { return DateTime.Now.Subtract(_lastTimeOnline).TotalSeconds <= 5 && _lastTimeOnline != DateTime.MinValue; }
            set { _lastTimeOnline = value ? DateTime.Now : DateTime.MinValue; }
        }

        private DateTime _lastTimeOnline = DateTime.MinValue;
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
        /// <summary>
        /// Serialized ChangeHistory
        /// </summary>
        public byte[] ChangeHistoryData {
            get
            {
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, ChangeHistory);
                    return stream.ToArray();
                }
            }
            set
            {
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream(value))
                {
                    ChangeHistory = (PhraseChangeHistory)formatter.Deserialize(stream);
                }
            } 
        }


        [Ignore]
        public PhraseChangeHistory ChangeHistory { get; set; }
    }

}
