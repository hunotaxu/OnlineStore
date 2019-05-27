//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OnlineStore.Services
//{
//    public interface ILocalizationService
//    {
//        /// <summary>
//        /// Gets a resource string based on the specified ResourceKey property.
//        /// </summary>
//        /// <param name="resourceKey">A string representing a ResourceKey.</param>
//        /// <param name="languageId">Language identifier</param>
//        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
//        /// <param name="defaultValue">Default value</param>
//        /// <param name="returnEmptyIfNotFound">A value indicating whether an empty string will be returned if a resource is not found and default value is set to empty string</param>
//        /// <returns>A string representing the requested resource string.</returns>
//        //string GetResource(string resourceKey, int languageId,
//        //    bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false);
//        string GetResource(string resourceKey,
//            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false);
//    }
//}
