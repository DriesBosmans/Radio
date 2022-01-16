﻿using MOB_RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using XamarinCountryPicker.Models;

namespace MOB_RadioApp.Services
{
    public static class CountryService

    {

        /// <summary>
        /// Gets the list of countries based on ISO 3166-1
        /// </summary>
        /// <returns>Returns the list of countries based on ISO 3166-1</returns>
        public static List<RegionInfo> GetCountriesByIso3166()
        {
            var countries = new List<RegionInfo>();
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                var info = new RegionInfo(culture.LCID);
                if (countries.All(p => p.Name != info.Name))
                    countries.Add(info);
            }
            return countries.OrderBy(p => p.EnglishName).ToList();
        }

        /// <summary>
        /// Get Country Model by Country Name
        /// </summary>
        /// <param name="countryName">English Name of Country</param>
        /// <returns>Complete Country Model with Region, Flag, Name and Code</returns>
        public static ObservableCollection<CountryModel> GetCountries()
        {

            var isoCountries = GetCountriesByIso3166();
            ObservableCollection<CountryModel> countries = new ObservableCollection<CountryModel>();
            foreach (var country in isoCountries)
            {
                var countrymode = new CountryModel
                {
                    CountryName = country.EnglishName,
                    CountryCode = country.TwoLetterISORegionName
                };
                countries.Add(countrymode);
            }
            return countries;


        }
    }
}