using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Filia.Shared
{
    public enum UserRole
    {
        Admin = 0,
        Moderator = 1,
        Translator = 2,
        Editor = 3,
        Reader = 4,
        Blocked = 5,
        Anonimus = 6
    }

    #region puublic interfaces

    public interface IUserData
    {
        int Id { get; }
        string Nickname { get; }
        string Realname { get; }
        string About { get; }
        string Email { get; }
        UserRole Role { get; }
        bool UploadImages { get; }
        bool Online { get; }
    }

    public interface IPhraseData
    {
        int Id { get; }
        string Original { get; }
        string Translated { get; }

        /// <summary>
        /// Will be changed only if user will change Translated field.
        /// </summary>
        string AuthorOfTranslation { get; }

        string Tags { get; }
        string Comment { get; }
        string ImageUrl { get; }
        string AuthorOfRevision { get; }
        PhraseChangeHistory ChangeHistory { get; }
    }

    #endregion

    #region For network

    [Serializable]
    public class UserInformation : IUserData
    {

        public UserInformation(int id, string nickname, string realname, string about, string email, UserRole role,
            bool uploadImages, bool online)
        {
            Id = id;
            Nickname = nickname;
            Realname = realname;
            About = about;
            Email = email;
            Role = role;
            UploadImages = uploadImages;
            Online = online;
        }

        public int Id { get; private set; }
        public string Nickname { get; private set; }
        public string Realname { get; private set; }
        public string About { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }
        public bool UploadImages { get; private set; }
        public bool Online { get; private set; }
    }


    [Serializable]
    public class ShortUserInformation : PropertyCopier
    {

        public ShortUserInformation(int id, string nickname, string realname, UserRole role, bool online)
        {
            Id = id;
            Nickname = nickname;
            Realname = realname;
            Role = role;
            Online = online;
        }

        public int Id { get; private set; }
        public string Nickname { get; private set; }
        public string Realname { get; private set; }
        public UserRole Role { get; private set; }
        public bool Online { get; private set; }


    }

    [Serializable]
    public class PhraseInformation : IPhraseData
    {
        public PhraseInformation(int id, string original, string translated, string authorOfTranslation, string tags,
            string comment, string imageUrl, string authorOfRevision, PhraseChangeHistory changeHistory)
        {
            Id = id;
            Original = original;
            Translated = translated;
            AuthorOfTranslation = authorOfTranslation;
            Tags = tags;
            Comment = comment;
            ImageUrl = imageUrl;
            AuthorOfRevision = authorOfRevision;
            ChangeHistory = changeHistory;
        }

        public int Id { get; private set; }
        public string Original { get; private set; }
        public string Translated { get; private set; }
        public string AuthorOfTranslation { get; private set; }
        public string Tags { get; private set; }
        public string Comment { get; private set; }
        public string ImageUrl { get; private set; }
        public string AuthorOfRevision { get; private set; }

        /// <summary>
        /// Will be sended to client by request
        /// </summary>
        public PhraseChangeHistory ChangeHistory { get; private set; }
    }

    #endregion

    /// <summary>
    /// History of revisions of the phrase
    /// </summary>
    [Serializable]
    public class PhraseChangeHistory
    {
        public PhraseChangeHistory(List<PhraseChangeHistoryRevision> revisions)
        {
            Revisions = revisions;
        }

        public List<PhraseChangeHistoryRevision> Revisions { get; private set; }
    }

    /// <summary>
    /// Revision of phrase. Fields are empty if this field wasn't been changed since the last time. And if they were, than it's containing their previous value.
    /// </summary>
    [Serializable]
    public class PhraseChangeHistoryRevision
    {
        public PhraseChangeHistoryRevision(Guid id, string translation, string authorOfTranslation, string tags,
            string comment, string imageUrl, string authorOfRevision)
        {
            Id = id;
            Translation = translation;
            AuthorOfTranslation = authorOfTranslation;
            Tags = tags;
            Comment = comment;
            ImageUrl = imageUrl;
            AuthorOfRevision = authorOfRevision;
        }

        public Guid Id { get; private set; }
        public string Translation { get; private set; }
        public string AuthorOfTranslation { get; private set; }
        public string Tags { get; private set; }
        public string Comment { get; private set; }
        public string ImageUrl { get; private set; }
        public string AuthorOfRevision { get; private set; }
    }

    /// <summary>
    /// Helper for copying properties & notifications
    /// </summary>
    [Serializable]
    public class PropertyCopier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            //C# 6 null-safe operator. No need to check for event listeners
            //If there are no listeners, this will be a noop
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void CopyProperties(object dest, object src)
        {
            foreach (
                var item in TypeDescriptor.GetProperties(src).Cast<PropertyDescriptor>().Where(item => !item.IsReadOnly)
                )
            {
                item.SetValue(dest, item.GetValue(src));
            }
        }

        public void CopyProperties(object src)
        {
            foreach (var item in src.GetType().GetProperties())
            {
                var t = this.GetType().GetProperty(item.Name);
                t.SetValue(this, item.GetValue(src, null), null);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item.Name));
            }
        }
    }
}
