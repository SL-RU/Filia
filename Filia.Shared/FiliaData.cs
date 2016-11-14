﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            bool uploadImages)
        {
            Id = id;
            Nickname = nickname;
            Realname = realname;
            About = about;
            Email = email;
            Role = role;
            UploadImages = uploadImages;
        }

        public int Id { get; private set; }
        public string Nickname { get; private set; }
        public string Realname { get; private set; }
        public string About { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }
        public bool UploadImages { get; private set; }
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
}
