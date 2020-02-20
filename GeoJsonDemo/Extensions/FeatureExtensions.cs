using System;
using System.Collections.Generic;
using GeoJSON.Net.Feature;

namespace GeoJsonDemo
{
    public static class FeatureExtensions
    {
        public static string GetStringProperty(this Feature feature, string propertyName, string defaultValue = "")
        {
            return feature != null && feature.Properties != null && feature.Properties.ContainsKey(propertyName) && feature.Properties[propertyName] is string ? (string)feature.Properties[propertyName] : defaultValue;
        }

        public static int GetIntegerProperty(this Feature feature, string propertyName, int defaultValue = 0)
        {
            return feature != null && feature.Properties != null && feature.Properties.ContainsKey(propertyName) && feature.Properties[propertyName] is int ? (int)feature.Properties[propertyName] : defaultValue;
        }

        public static long GetLongProperty(this Feature feature, string propertyName, long defaultValue = 0)
        {
            return feature != null && feature.Properties != null && feature.Properties.ContainsKey(propertyName) && feature.Properties[propertyName] is long ? (long)feature.Properties[propertyName] : defaultValue;
        }

        public static bool GetBooleanProperty(this Feature feature, string propertyName, bool defaultValue = false)
        {
            return feature != null && feature.Properties != null && feature.Properties.ContainsKey(propertyName) && feature.Properties[propertyName] is bool ? (bool)feature.Properties[propertyName] : defaultValue;
        }
    }
}
